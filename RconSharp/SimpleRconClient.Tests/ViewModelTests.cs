using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RconSharp;
using SimpleRconClient.ViewModel;
using SimpleRconClient.ViewModel.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRconClient.Tests
{
	[TestClass]
	public class ViewModelTests
	{
		[TestMethod]
		[TestCategory("MainViewModel")]
		public async Task ConnectAndAuthenticate()
		{ 
			// Arrange
			var mainViewModel = InitializeViewModel(new byte[] { 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00 });
			mainViewModel.Host = "remotehost.com";
			mainViewModel.Port = 12345;
			mainViewModel.Password = "supersecretpassword";

			// Act
			var command = mainViewModel.ConnectCommand as AsyncRelayCommand;
			await command.ExecuteAsync(null);
			
			// Assert
			Assert.IsTrue(mainViewModel.IsConnected);
			Assert.IsFalse(mainViewModel.IsError);
			Assert.IsFalse(mainViewModel.IsWorking);
		}

		[TestMethod]
		[TestCategory("MainViewModel")]
		public async Task ConnectWithWrongCredentials()
		{
			// Arrange
			var mainViewModel = InitializeViewModel(new byte[] { 0x0a, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00 });
			mainViewModel.Host = "remotehost.com";
			mainViewModel.Port = 12345;
			mainViewModel.Password = "supersecretpassword";

			// Act
			var command = mainViewModel.ConnectCommand as AsyncRelayCommand;
			await command.ExecuteAsync(null);

			// Assert
			Assert.IsFalse(mainViewModel.IsConnected);
			Assert.IsTrue(mainViewModel.IsError);
			Assert.IsTrue(mainViewModel.ErrorMessage.Equals("Wrong password. Unable to authenticate"));
			Assert.IsFalse(mainViewModel.IsWorking);
		}

		private MainViewModel InitializeViewModel(byte[] fakeResponse)
		{
			Mock<INetworkSocket> socket = new Mock<INetworkSocket>();
			socket.Setup(s => s.ConnectAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);
			socket.Setup(s => s.SendDataAndReadResponseAsync(It.IsAny<byte[]>())).ReturnsAsync(fakeResponse);
			socket.SetupGet(s => s.IsConnected).Returns(false);
			RconMessenger messenger = new RconMessenger(socket.Object);
			return new MainViewModel(messenger);
		}
	}
}
