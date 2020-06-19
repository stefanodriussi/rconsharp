using System;
using System.ComponentModel;
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
	public class CoreTests
	{
		[Fact, Category("Packets")]
		public void BuildPacket()
		{ 
			// Arrange
			var packet = new RconPacket(PacketType.Auth, "password");

			// Act
			var buffer = packet.GetBytes();

			// Asert
			Assert.NotNull(buffer);
			Assert.True(buffer.Length == 22);  // 14 fixed bytes + 8 bytes for "password" string encoding
			Assert.True(buffer[0] == 0x12);
			Assert.True(buffer[buffer.Length - 1] == 0x00 && buffer[buffer.Length - 2] == 0x00);
		}

		[Fact, Category("Packets")]
		public void BuildPacketWithoutType()
		{
			// Arrange

			// Act
			Assert.Throws<ArgumentException>(() => new RconPacket(null, "password"));
		}

		[Fact, Category("Packets")]
		public void BuildPacketWithEmptyContent()
		{
			// Arrange
			var packet = new RconPacket(PacketType.Auth, string.Empty);

			// Act
			var buffer = packet.GetBytes();

			// Asert
			Assert.NotNull(buffer);
			Assert.True(buffer.Length == 14);  // 14 fixed bytes
			Assert.True(buffer[buffer.Length - 1] == 0x00 && buffer[buffer.Length - 2] == 0x00);
		}

		[Fact, Category("Packets")]
		public void BuildPacketFromBytes()
		{
			// Arrange
			var originalPacket = new RconPacket(PacketType.Auth, "password");

			// Act
			var buffer = originalPacket.GetBytes();
			var parsedPacket = RconPacket.FromBytes(buffer);

			// Assert
			Assert.True(parsedPacket.Body.Equals(originalPacket.Body));
			Assert.True(parsedPacket.Id.Equals(originalPacket.Id));
		}
	}
}
