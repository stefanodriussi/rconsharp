using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
	public class CoreTests
	{
		[TestCategory("Packets")]
		[TestMethod]
		public void BuildPacket()
		{ 
			// Prepare
			var packet = new RconPacket(PacketType.Auth, "password");

			// Act
			var buffer = packet.GetBytes();

			// Asert
			Assert.IsNotNull(buffer);
			Assert.IsTrue(buffer.Length == 22);  // 14 fixed bytes + 8 bytes for "password" string encoding
			Assert.IsTrue(buffer[0] == 0x12);
			Assert.IsTrue(buffer[buffer.Length - 1] == 0x00 && buffer[buffer.Length - 2] == 0x00);
		}

		[TestCategory("Packets")]
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void BuildPacketWithoutType()
		{
			// Prepare
			var packet = new RconPacket(null, "password");

			// Act
			var buffer = packet.GetBytes();
		}

		[TestCategory("Packets")]
		[TestMethod]
		public void BuildPacketWithEmptyContent()
		{
			// Prepare
			var packet = new RconPacket(PacketType.Auth, string.Empty);

			// Act
			var buffer = packet.GetBytes();

			// Asert
			Assert.IsNotNull(buffer);
			Assert.IsTrue(buffer.Length == 14);  // 14 fixed bytes
			Assert.IsTrue(buffer[buffer.Length - 1] == 0x00 && buffer[buffer.Length - 2] == 0x00);
		}

		[TestCategory("Packets")]
		[TestMethod]
		public void BuildPacketFromBytes()
		{ 
			// Prepare
			var originalPacket = new RconPacket(PacketType.Auth, "password");

			// Act
			var buffer = originalPacket.GetBytes();
			var parsedPacket = RconPacket.FromBytes(buffer);

			// Assert
			Assert.IsTrue(parsedPacket.Body.Equals(originalPacket.Body));
			Assert.IsTrue(parsedPacket.Id.Equals(originalPacket.Id));
		}
	}
}
