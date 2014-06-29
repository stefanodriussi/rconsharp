using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			socket.Setup(s => s.ConnectAsync()).ReturnsAsync(true);
			socket.Setup(s => s.SendDataAndReadResponseAsync(It.IsAny<byte[]>())).ReturnsAsync(fakeResponse);
			socket.SetupGet<bool>(s => s.IsConnected).Returns(isConnected);
			return new RconMessenger(socket.Object);
		}
	}
}
