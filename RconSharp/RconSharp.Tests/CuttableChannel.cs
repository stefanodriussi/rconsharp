using System;
using System.Threading;
using System.Threading.Tasks;

namespace RconSharp.Tests {
	public class CuttableChannel : IChannel {
		private bool isCut = false;
		
		public bool IsConnected { get; private set; } = false;
		
		public void CutConnection() {
			isCut = true;
		}
		
		public Task ConnectAsync(CancellationToken cancellationToken) {
			if (isCut) {
				return Task.Delay(-1, cancellationToken);
			}
			IsConnected = true;
			return Task.CompletedTask;
		}

		public void Disconnect() {
			IsConnected = false;
			isCut = false;
		}
		
		public Task SendAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken) {
			if (isCut) {
				return Task.Delay(-1, cancellationToken);
			}
			return Task.CompletedTask;
		}
		
		public async Task<int> ReceiveAsync(Memory<byte> responseBuffer, CancellationToken cancellationToken) {
			if (isCut) {
				await Task.Delay(-1, cancellationToken);
			}
			// Should be positive, and responseBuffer should get written to
			// However I don't actually know how to write to Memory<T>, and that isn't part of the tests performed on this class anyway.
			return 0;
		}
	}
}
