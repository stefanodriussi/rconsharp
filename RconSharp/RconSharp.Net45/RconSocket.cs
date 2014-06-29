using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RconSharp.Net45
{
	public class RconSocket : INetworkSocket
	{
		private TcpClient _client;
		private string _host;
		private int _port;

		public RconSocket(string host, int port)
		{
			if (string.IsNullOrEmpty(host))
				throw new ArgumentException("Invalid host name: must be a non null non empty string containing the host's address");

			if (port < 1)
				throw new ArgumentException("Port parameter must be a positive value");

			_host = host;
			_port = port;
		}

		public async Task<bool> ConnectAsync()
		{
			if (_client == null)
				_client = new TcpClient(AddressFamily.InterNetwork);

			if (_client.Connected)
				return false;

			await _client.ConnectAsync(_host, _port);
			return true;
		}

		public bool IsConnected
		{
			get { return _client == null ? false : _client.Connected; }
		}

		public async Task<byte[]> SendDataAndReadResponseAsync(byte[] data)
		{
			if (!IsConnected)
				throw new InvalidOperationException("You must call Connect() method before seinding data");

			NetworkStream ns = _client.GetStream();
			await ns.WriteAsync(data, 0, data.Length);
			await ns.FlushAsync();
			byte[] buffer = new byte[4096];
			int offset = 0;
			while (ns.DataAvailable)
			{
				offset += await ns.ReadAsync(buffer, offset, buffer.Length);
			}
			byte[] result = new byte[offset];
			Array.Copy(buffer, 0, result, 0, offset);
			return result;
		}


		public void CloseConnection()
		{
			if (_client != null && _client.Connected)
			{
				_client.Close();
			}
		}

	}
}
