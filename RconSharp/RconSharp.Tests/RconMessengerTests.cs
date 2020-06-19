using Moq;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xunit;

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

namespace RconSharp.Tests
{
	public class RconMessengerTests
	{
		[Fact, Category("RconMessenger")]
		public async Task AuthenticateWithCorrectPassword()
		{
			// Arrange
			byte[] fakeResponse = new byte[] { 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,  0x02, 0x00, 0x00, 0x00, 0x00, 0x00  };
			var messenger = SetupMessenger(fakeResponse, false);

			// Act
			bool response = await messenger.AuthenticateAsync("correctPassword");

			// Assert
			Assert.True(response);
		}

		[Fact, Category("RconMessenger")]
		public async Task AuthenticateWithInvalidPassword()
		{
			// Arrange
			byte[] fakeResponse = new byte[] { 0x0a, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00 };
			var messenger = SetupMessenger(fakeResponse, true);

			// Act
			bool response = await messenger.AuthenticateAsync("correctPassword");

			// Assert
			Assert.False(response);
		}

		[Fact, Category("RconMessenger")]
		public async Task SendCommandWithoutAuthentication()
		{
			// Arrange
			var messenger = SetupMessenger(null, false);

			// Act and Assert
			try
			{
				var response = await messenger.ExecuteCommandAsync("somecommand");
				Assert.NotNull(response);
			}
			catch (InvalidOperationException)
			{ 
			}
		}

		[Fact, Category("RconMessenger")]
		public async Task SendEmptyCommand()
		{
			// Arrange
			var messenger = SetupMessenger(null, false);

			// Act and Assert
			try
			{
				var response = await messenger.ExecuteCommandAsync(string.Empty);
				Assert.NotNull(response);
			}
			catch (ArgumentException)
			{
			}
		}

		[Fact, Category("RconMessenger")]
		public async Task SendValidCommand()
		{
			// Arrange
			byte[] fakeResponse = new RconPacket(PacketType.ResponseValue, "command response").GetBytes();
			var messenger = SetupMessenger(fakeResponse, true);

			// Act
			var response = await messenger.ExecuteCommandAsync("command");

			// Assert
			Assert.Equal("command response", response);
		}

		private RconMessenger SetupMessenger(byte[] fakeResponse, bool isConnected)
		{
			Mock<INetworkSocket> socket = new Mock<INetworkSocket>();
			socket.Setup(s => s.ConnectAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);
			socket.Setup(s => s.SendDataAndReadResponseAsync(It.IsAny<byte[]>())).ReturnsAsync(fakeResponse);
			socket.SetupGet<bool>(s => s.IsConnected).Returns(isConnected);
			return new RconMessenger(socket.Object);
		}
	}
}
