using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RconSharp.Tests
{
    public class FakeChannel : IChannel
    {
        public FakeChannel()
        {
            responsesMap = new Dictionary<PacketType, byte[]>
            {
                { PacketType.ExecCommand, RconPacket.Create(PacketType.Response, "Command executed").ToBytes() },
                { PacketType.Auth, RconPacket.Create(PacketType.AuthResponse, "").ToBytes() }
            };
        }

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
            var pendingCommand = await Task.Run(() => incomingRequests.Take());
            if (cancelNextReponse) throw new Exception("Canceled");
            var nextResponse = responsesMap[pendingCommand.Type];
            nextResponse.CopyTo(responseBuffer);
            return nextResponse.Length;
        }
        private BlockingCollection<RconPacket> incomingRequests = new BlockingCollection<RconPacket>();

        public Task SendAsync(ReadOnlyMemory<byte> payload)
        {
            incomingRequests.Add(RconPacket.FromBytes(payload.ToArray()));
            return Task.CompletedTask;
        }
        private bool cancelNextReponse = false;
        private Dictionary<PacketType, byte[]> responsesMap;
        public void CancelNextReponse() => cancelNextReponse = true;
    }
}
