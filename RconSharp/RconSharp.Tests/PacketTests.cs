using System;
using System.Text;
using Xunit;

namespace RconSharp.Tests
{
    public class PacketTests
	{
		[Fact]
		public void ConvertRconPacketToBytesShouldReturnValidRCONBuffer()
		{
			// Arrange
			ProgressiveId.Seed(1);
			var content = "test";
			var packetType = PacketType.Auth;
			var bufferLength = 18;
			var packetLength = 14;
			var packet = RconPacket.Create(packetType, content);

			// Act
			var buffer = packet.ToBytes();

			// Asert
			Assert.NotNull(buffer);
			Assert.Equal(bufferLength, buffer.Length);  // 12 headers bytes + 8 bytes for "password" string encoding + 1 ending
			Assert.Equal(packetLength, BitConverter.ToInt32(buffer[0..4]));
			Assert.Equal(1, BitConverter.ToInt32(buffer[4..8]));
			Assert.Equal(packetType, (PacketType)BitConverter.ToInt32(buffer[8..12]));
			Assert.Equal(content, Encoding.UTF8.GetString(buffer[12..^2]));
			Assert.True(buffer[^1] == 0x00);
		}

		[Theory]
		[InlineData("", PacketType.Auth, 15, 11)]
		[InlineData(null, PacketType.Auth, 15, 11)]
		public void ConvertEmptyContentRconPacketToBytesShouldReturnValidRCONBuffer(string content, PacketType packetType, int bufferLength, int packetLength)
		{
			// Arrange
			ProgressiveId.Seed(1);
			var packet = RconPacket.Create(packetType, content);

			// Act
			var buffer = packet.ToBytes();

			// Asert
			Assert.NotNull(buffer);
			Assert.Equal(bufferLength, buffer.Length);  // 12 headers bytes + 8 bytes for "password" string encoding + 1 ending
			Assert.Equal(packetLength, BitConverter.ToInt32(buffer[0..4]));
			Assert.Equal(1, BitConverter.ToInt32(buffer[4..8]));
			Assert.Equal(packetType, (PacketType)BitConverter.ToInt32(buffer[8..12]));
			Assert.Equal("\0", Encoding.UTF8.GetString(buffer[12..^2]));
			Assert.True(buffer[^1] == 0x00);
		}

		[Theory]
		[InlineData(new byte[] { 17,0,0,0,1,0,0,0,3,0,0,0,112,97,115,115,119,111,114,100, 0,0 }, 17, 1, PacketType.Auth, "password")]
		[InlineData(new byte[] { 10,0,0,0,1,0,0,0,3,0,0,0,0, 0,0 }, 10, 1, PacketType.Auth, "")]
        public void ConvertByteArrayShouldReturnValidRCONPacket(byte[] buffer, int size, int id, PacketType type, string body)
		{
			// Arrange


			// Act
			var packet = RconPacket.FromBytes(buffer);

			// Asert
			Assert.Equal(id, packet.Id);
			Assert.Equal(body, packet.Body);
			Assert.Equal(size, packet.Size);
			Assert.Equal(type, packet.Type);
		}

		[Theory]
		[InlineData(null, typeof(ArgumentNullException))]
		[InlineData(new byte[] { 1 }, typeof(ArgumentException))]
		[InlineData(new byte[] { 100, 0, 0, 0, 0 }, typeof(ArgumentOutOfRangeException))]
		public void ConvertByteArrayShouldThrowIfInvalidBufferIsPassed(byte[] buffer, Type exception)
        {
			// Arrange

			// Act
			Assert.Throws(exception, () => RconPacket.FromBytes(buffer));
		}
	}
}
