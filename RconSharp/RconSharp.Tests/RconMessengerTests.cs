using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
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

namespace RconSharp.Tests
{
	[TestClass]
	public class RconMessengerTests
	{
		[TestMethod]
		[TestCategory("RconMessenger")]
		public async Task AuthenticateWithCorrectPassword()
		{ 
			// Prepare
			byte[] fakeResponse = new byte[] { 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,  0x02, 0x00, 0x00, 0x00, 0x00, 0x00  };
			var messenger = SetupMessenger(fakeResponse, false);

			// Act
			bool response = await messenger.AuthenticateAsync("correctPassword");

			// Assert
			Assert.IsTrue(response);
		}

		[TestMethod]
		[TestCategory("RconMessenger")]
		public async Task AuthenticateWithInvalidPassword()
		{
			// Prepare
			byte[] fakeResponse = new byte[] { 0x0a, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00 };
			var messenger = SetupMessenger(fakeResponse, true);

			// Act
			bool response = await messenger.AuthenticateAsync("correctPassword");

			// Assert
			Assert.IsFalse(response);
		}

		[TestMethod]
		[TestCategory("RconMessenger")]
		public async Task SendCommandWithoutAuthentication()
		{ 
			// Prepare
			var messenger = SetupMessenger(null, false);

			// Act and Assert
			try
			{
				var response = await messenger.ExecuteCommandAsync("somecommand");
				Assert.Fail();
			}
			catch (InvalidOperationException)
			{ 
			}
		}

		[TestMethod]
		[TestCategory("RconMessenger")]
		public async Task SendEmptyCommand()
		{
			// Prepare
			var messenger = SetupMessenger(null, false);

			// Act and Assert
			try
			{
				var response = await messenger.ExecuteCommandAsync(string.Empty);
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}
		}

		[TestMethod]
		[TestCategory("RconMessenger")]
		public async Task SendValidCommand()
		{
			// Prepare
			byte[] fakeResponse = new RconPacket(PacketType.ResponseValue, "command response").GetBytes();
			var messenger = SetupMessenger(fakeResponse, true);

			// Act
			var response = await messenger.ExecuteCommandAsync("command");

			// Assert
			Assert.AreEqual("command response", response);
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
