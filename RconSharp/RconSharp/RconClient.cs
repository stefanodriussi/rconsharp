using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

/*
The MIT License (MIT)

Copyright (c) 2014 Stefano Driussi

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace RconSharp
{
	/// <summary>
	/// Rcon protocol messages handler
	/// </summary>
	public class RconClient
	{
		private IChannel channel;
		private Pipe pipe = new Pipe();

		private Encoding encoding;

		private Queue<Operation> operations = new Queue<Operation>();
		private Task communicationTask;
		public event Action ConnectionClosed;
        public static RconClient Create(string host, int port) => new RconClient(new SocketChannel(host, port));
        public static RconClient Create(IChannel channel) => new RconClient(channel);
		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="channel">a NetworkSocket implementation</param>
		private RconClient(IChannel channel)
		{
			if (channel == null)
				throw new NullReferenceException("channel parameter must be an instance of a class implementing INetworkSocket inteface");
			this.channel = channel;
			encoding = Encoding.UTF8;
		}

		public RconClient WithEncoding(Encoding encoding)
		{
			this.encoding = encoding;
			return this;
		}

		public string Host { get; private set; }
		public int Port { get; private set; }

		/// <summary>
		/// Connect the socket to the remote endpoint
		/// </summary>
		/// <returns>True if the connection was successfull; False if a connection was already estabilished</returns>
		public async Task ConnectAsync()
		{
			if (channel.IsConnected) return;
			await channel.ConnectAsync();
			var readingTask = ReadFromPipeAsync(pipe.Reader);
			var writingTask = WriteToPipeAsync(pipe.Writer);
			communicationTask = Task.WhenAll(readingTask, writingTask).ContinueWith(t =>
			{
				pipe.Reset();
				while (operations.TryDequeue(out var result))
				{
					result.TaskCompletionSource.SetCanceled();
				}
				ConnectionClosed?.Invoke();
			});
        }

		public void Disconnect() => channel.Disconnect();

        private async Task WriteToPipeAsync(PipeWriter writer)
        {
			while (true)
			{
				var buffer = writer.GetMemory(14);
				try
				{
					var bytesCount = await channel.ReceiveAsync(buffer);
					if (bytesCount == 0) break;

					writer.Advance(bytesCount);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"[{nameof(WriteToPipeAsync)}] Exception: {ex.Message}");
					break;
				}

				var flushResult = await writer.FlushAsync();
				if (flushResult.IsCompleted) break;

			}
			await writer.CompleteAsync();

			//client is disconnected
        }

		private async Task ReadFromPipeAsync(PipeReader reader)
		{
			while (true)
			{
				var readResult = await reader.ReadAsync();
				var buffer = readResult.Buffer;
				var startPosition = buffer.Start;
				if (buffer.Length < 4) // not enough bytes to get the packet length, need to read more
				{
					if (readResult.IsCompleted) break;

					reader.AdvanceTo(startPosition, buffer.End);
					continue;
				}

				var packetSize = BitConverter.ToInt32(buffer.Slice(startPosition, 4).ToArray());
				if (buffer.Length >= packetSize + 4)
				{
					var endPosition = buffer.GetPosition(packetSize + 4, startPosition);
					var rconPacket = RconPacket.FromBytes(buffer.Slice(startPosition, endPosition).ToArray(), encoding);
					if (!rconPacket.IsDummy)
					{
						var currentOperation = operations.Peek();
						currentOperation.Add(rconPacket);
						if (currentOperation.OriginalRequest.Type == PacketType.Auth && rconPacket.Type == PacketType.Response)
						{
							// According to RCON documentation an empty RESPONSE packet must be sent after the auth request, but at the moment only CS:GO does so ..
						}
						else if (currentOperation.IsMultiPacketResponse && !string.IsNullOrEmpty(rconPacket.Body))
						{
							// Accumulate and move on
						}
						else
						{
							operations.Dequeue();
							if (rconPacket.Id == -1) currentOperation.TaskCompletionSource.SetException(new AuthenticationException("Invalid password"));
							else currentOperation.TaskCompletionSource.SetResult(currentOperation.Body);
						}
					}
					
					reader.AdvanceTo(endPosition);
				}
				else
				{
					reader.AdvanceTo(startPosition, buffer.End);
				}

				if (buffer.IsEmpty && readResult.IsCompleted) break;
			}

			await reader.CompleteAsync();
		}

		/// <summary>
		/// Send the proper authentication packet and parse the response
		/// </summary>
		/// <param name="password">Current server password</param>
		/// <returns>True if the connection has been authenticated; False elsewhere</returns>
		/// <remarks>This method must be called prior to sending any other command</remarks>
		/// <exception cref="ArgumentException">Is thrown if <paramref name="password"/> parameter is null or empty</exception>
		public async Task<bool> AuthenticateAsync(string password)
		{
			if (string.IsNullOrEmpty(password))
				throw new ArgumentException("password parameter must be a non null non empty string");

			var authPacket = RconPacket.Create(PacketType.Auth, password);
			try
			{
				var response = await SendPacketAsync(authPacket);
			}
			catch (AuthenticationException)
			{
				return false;
			}
			return true;
		}

		public Task<string> ExecuteCommandAsync(string command, bool isMultiPacketResponse = false)
		{
			return SendPacketAsync(RconPacket.Create(PacketType.ExecCommand, command), isMultiPacketResponse);
		}

		/// <summary>
		/// Send a message encapsulated into an Rcon packet and get the response
		/// </summary>
		/// <param name="packet">Packet to be sent</param>
		/// <returns>The response to this command</returns>
		private async Task<string> SendPacketAsync(RconPacket packet, bool isMultiPacketResponse = false)
		{
			var packetDescription = new Operation(packet, isMultiPacketResponse);
			operations.Enqueue(packetDescription);
			await channel.SendAsync(packet.ToBytes(encoding));
			if (isMultiPacketResponse)
			{
				await channel.SendAsync(RconPacket.Dummy.Value.ToBytes(encoding));
			}

			return await packetDescription.TaskCompletionSource.Task;
		}

		private class Operation
		{
			public Operation(RconPacket originalRequest, bool isMultiPacketResponse)
			{
				OriginalRequest = originalRequest;
				TaskCompletionSource = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
				IsMultiPacketResponse = isMultiPacketResponse;
			}
			public TaskCompletionSource<string> TaskCompletionSource { get; }
			public RconPacket OriginalRequest { get; }
			public bool IsMultiPacketResponse { get; }
			private List<RconPacket> PacketsBuffer = new List<RconPacket>();
			public void Add(RconPacket packet) => PacketsBuffer.Add(packet);
			public string Body => string.Concat(PacketsBuffer.Select(b => b.Body));
		}
	}
}
