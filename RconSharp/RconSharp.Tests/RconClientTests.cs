using System;
using System.Threading;
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
            var response = await rconClient.ExecuteCommandAsync("test echo");

            // Assert
            Assert.Equal("Command executed", response);
        }

        [Fact]
        public async Task CanceledResponseShouldDisconnectTheClient()
        {
            // Arrange
            var channel = new FakeChannel();
            channel.CancelNextReponse();
            var rconClient = RconClient.Create(channel);
            await rconClient.ConnectAsync();
            var tcs = new TaskCompletionSource<bool>();
            rconClient.ConnectionClosed += () =>
            {
                tcs.SetResult(true);
            };

            // Act Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => rconClient.ExecuteCommandAsync("test echo"));
            Assert.True(await tcs.Task);
        }

        [Theory]
        [InlineData("valid", true)]
        [InlineData("invalid", false)]
        public async Task AuthenticateShouldWorkOnStrictRCONServers(string password, bool isAuthenticated)
        {
            // Arrange
            var channel = new SourceChannel();
            var rconClient = RconClient.Create(channel);
            await rconClient.ConnectAsync();

            // Act
            var response = await rconClient.AuthenticateAsync(password, default);

            // Assert
            Assert.Equal(isAuthenticated, response);
        }

        [Fact]
        public async Task MultiPacketResponseShouldBeCorrectlyReceivedFromStrictRCONServers()
        {
            // Arrange
            var channel = new SourceChannel();
            var rconClient = RconClient.Create(channel);
            await rconClient.ConnectAsync();

            // Act
            var response = await rconClient.ExecuteCommandAsync("print all", true);

            // Assert
            Assert.Equal("This will be a very long message", response);
        }

        [Fact]
        public async Task OperationsShouldThrowAfterTimeout()
        {
            var channel = new CuttableChannel();
            var rconClient = RconClient.Create(channel);
            CancellationTokenSource cts;
            
            // Cut before ConnectAsync
            channel.CutConnection();
            cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            await Assert.ThrowsAsync<TaskCanceledException>(() => rconClient.ConnectAsync(cts.Token));
            rconClient.Disconnect();
            
            // Cut before AuthenticateAsync
            await rconClient.ConnectAsync();
            channel.CutConnection();
            cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            await Assert.ThrowsAsync<TaskCanceledException>(() => rconClient.AuthenticateAsync("password", cts.Token));
            rconClient.Disconnect();
            
            // Cut before ExecuteCommandAsync
            await rconClient.ConnectAsync();
            await rconClient.AuthenticateAsync("password");
            channel.CutConnection();
            cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            await Assert.ThrowsAsync<TaskCanceledException>(() => rconClient.ExecuteCommandAsync("command", cancellationToken: cts.Token));
        }
    }
}
