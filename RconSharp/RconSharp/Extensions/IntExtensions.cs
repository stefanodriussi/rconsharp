using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RconSharp.Extensions
{
	public static class IntExtensions
	{
		public static byte[] ToLittleEndian(this int value)
		{
			var bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
				Array.Reverse(bytes);
			return bytes;
		}

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
