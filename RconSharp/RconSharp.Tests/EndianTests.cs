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
using System;
using RconSharp.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RconSharp.Tests
{
	[TestClass]
	public class EndianTests
	{
		[TestCategory("Little Endian Conversion")]
		[TestMethod]
		public void ConvertIntToLittleEndian()
		{
			// Prepare
			int value = 17;

			// Act
			byte[] bytes = value.ToLittleEndian();

			// Assert
			Assert.IsTrue(bytes.Length == 4);
			Assert.IsTrue(bytes[0] == 0x11);
		}

		[TestCategory("Little Endian Conversion")]
		[TestMethod]
		public void ConvertLittleEndianToInt()
		{
			// Prepare
			byte[] bytes = new byte[] { 0x11, 0x00, 0x00, 0x00 };

			// Act
			int value = bytes.ToInt32(0);

			// Assert
			Assert.AreEqual(value, 17);
		}

		[TestCategory("Little Endian Conversion")]
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConvertLittleEndianToIntInvalidInput()
		{
			// Prepare
			byte[] bytes = new byte[] { 0x11, 0x00, 0x00 };

			// Act
			int value = bytes.ToInt32(0);

			// Assert
		}
	}
}
