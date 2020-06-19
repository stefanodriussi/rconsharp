using RconSharp.Extensions;
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
	public class EndianTests
	{
		[Fact, Category("Little Endian Conversion")]
		public void ConvertIntToLittleEndian()
		{
			// Arrange
			int value = 17;

			// Act
			byte[] bytes = value.ToLittleEndian();

			// Assert
			Assert.True(bytes.Length == 4);
			Assert.True(bytes[0] == 0x11);
		}

		[Fact, Category("Little Endian Conversion")]
		public void ConvertLittleEndianToInt()
		{
			// Arrange
			byte[] bytes = new byte[] { 0x11, 0x00, 0x00, 0x00 };

			// Act
			int value = bytes.ToInt32(0);

			// Assert
			Assert.Equal(17, value);
		}

		[Fact, Category("Little Endian Conversion")]
		public void ConvertLittleEndianToIntInvalidInput()
		{
			// Arrange
			byte[] bytes = new byte[] { 0x11, 0x00, 0x00 };

			// Act
			Assert.Throws<ArgumentException>(() => bytes.ToInt32(0));

		}
	}
}
