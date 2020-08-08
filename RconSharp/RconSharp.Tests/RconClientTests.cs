using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RconSharp.Tests
{
    public class RconClientTests
    {
        [Fact]
        public async Task SendMessageShouldReceiveAResponse()
        {
            // Arrange
            var channel = new FakeChannel();
            var rconClient = RconClient.Create(channel);
            await rconClient.ConnectAsync();

            // Act
            var response = await rconClient.SendPacketAsync(RconPacket.Create(PacketType.ExecCommand, "test echo"));

            // Assert
            Assert.Equal(PacketType.Response, response.Type);
            Assert.Equal("Command executed", response.Body);
        }

        [Fact]
        public async Task CanceledResponseShouldDisconnectTheClient()
        {
            // Arrange
            var channel = new FakeChannel();
            channel.CancelNextReponse();
            var rconClient = RconClient.Create(channel);
            await rconClient.ConnectAsync();
            var isConnectionClosed = false;
            rconClient.ConnectionClosed += () =>
            {
                isConnectionClosed = true;
            };

            // Act Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => rconClient.SendPacketAsync(RconPacket.Create(PacketType.ExecCommand, "test echo")));
            Assert.True(isConnectionClosed);
        }
    }
}
