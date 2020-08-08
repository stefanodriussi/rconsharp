using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
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
	/// Tcp implementation of <see cref="IChannel"/> interface
	/// </summary>
	public class SocketChannel : IChannel
	{
		private Socket socket;
		private string host;
		private int port;
		private int readTimeout;
		private int writeTimeout;
		public SocketChannel(string host, int port, int readTimeout = 5000, int writeTimeout = 5000)
		{
			if (string.IsNullOrEmpty(host)) throw new ArgumentException("Invalid host name: must be a non null non empty string containing the host's address");

			if (port < 1) throw new ArgumentException("Port parameter must be a positive value");

			if (readTimeout < 0) throw new ArgumentException("Read timeout parameter must be a positive value");
			if (writeTimeout < 0) throw new ArgumentException("Write timeout parameter must be a positive value");
			this.host = host;
			this.port = port;
			this.readTimeout = readTimeout;
			this.writeTimeout = writeTimeout;
		}
		/// <summary>
		/// Connect the socket to the remote endpoint
		/// </summary>
		/// <returns>True if the connection was successfully; False if the connection is already estabilished</returns>
		/// <exception cref="ArgumentException">is thrown when host parameter is null or empty, or when port parameter value is less than 1</exception>
		public async Task ConnectAsync()
		{
			if (socket != null)
				return;

			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.ReceiveTimeout = 5000;
            socket.SendTimeout = 5000;
            socket.NoDelay = true;

			await socket.ConnectAsync(host, port).ConfigureAwait(false);
		}

		public async Task SendAsync(ReadOnlyMemory<byte> payload)
		{
			await socket.SendAsync(payload, SocketFlags.None).ConfigureAwait(false);
		}

		public async Task<int> ReceiveAsync(Memory<byte> responseBuffer)
		{
			return await socket.ReceiveAsync(responseBuffer, SocketFlags.None).ConfigureAwait(false);
		}

		public void Disconnect()
		{
			socket?.Close();
			socket?.Dispose();
			socket = null;
		}

		public bool IsConnected => socket?.Connected ?? false;
	}
}
