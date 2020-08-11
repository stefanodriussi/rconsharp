using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace RconSharp.Tests
{
    public class SourceChannel : IChannel
    {
        public bool IsConnected
        {
            get;
            private set;
        }

        public Task ConnectAsync()
        {
            IsConnected = true;
            return Task.CompletedTask;
        }

        public void Disconnect()
        {
            IsConnected = false;
        }

        public async Task<int> ReceiveAsync(Memory<byte> responseBuffer)
        {
            var pendingResponse = await Task.Run(() => pendingResponses.Take());
            pendingResponse.CopyTo(responseBuffer);
            return pendingResponse.Length;
        }

        public Task SendAsync(ReadOnlyMemory<byte> payload)
        {
            var request = RconPacket.FromBytes(payload.ToArray());
            switch (request.Type)
            {
                case PacketType.Auth:
                    pendingResponses.Add(RconPacket.Create(PacketType.Response, "").ToBytes());
                    pendingResponses.Add(RconPacket.Create(request.Body.Equals("valid") ? request.Id : -1, PacketType.AuthResponse, "").ToBytes());
                    break;
                case PacketType.ExecCommand:
                    pendingResponses.Add(RconPacket.Create(PacketType.Response, "This will be a ve").ToBytes());
                    pendingResponses.Add(RconPacket.Create(PacketType.Response, "ry long message").ToBytes());
                    break;
                case PacketType.Response:
                    pendingResponses.Add(RconPacket.Create(0, PacketType.Response, "").ToBytes());
                    pendingResponses.Add(RconPacket.Create(0, PacketType.Response, "\u0001").ToBytes());
                    break;

            }
            return Task.CompletedTask;
        }
        private BlockingCollection<byte[]> pendingResponses = new BlockingCollection<byte[]>();
    }
}
