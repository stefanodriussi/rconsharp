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

namespace RconSharp.Extensions
{
	/// <summary>
	/// Extensions for handling LittleEndian conversion
	/// </summary>
	public static class IntExtensions
	{
		/// <summary>
		/// Converts an Int32 value to a Little Endian 4 bytes array 
		/// </summary>
		/// <param name="value">Value to be converted</param>
		/// <returns>Array containing the Little Endian bytes</returns>
		public static byte[] ToLittleEndian(this int value)
		{
			var bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
				Array.Reverse(bytes);
			return bytes;
		}
		/// <summary>
		/// Converts a 4 bytes Little Endian array to an Int32
		/// </summary>
		/// <param name="buffer">Origin array</param>
		/// <param name="startIndex">Index of the 4 bytes value inside origin array</param>
		/// <returns>The Int32 value</returns>
		public static int ToInt32(this byte[] buffer, int startIndex)
		{
			if (startIndex + 4 > buffer.Length)
				throw new ArgumentException("Error: not enough bytes to perform the conversion");

			var bytes = new byte[4];
			Array.Copy(buffer, startIndex, bytes, 0, 4);
			if (!BitConverter.IsLittleEndian)
				Array.Reverse(bytes);

			return BitConverter.ToInt32(bytes, 0);
		}
	}
}
