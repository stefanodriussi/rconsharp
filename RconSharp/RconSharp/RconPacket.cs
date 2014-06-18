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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RconSharp.Extensions;

namespace RconSharp
{
	public class RconPacket
	{
		private const int sizeIndex = 0;
		private const int idIndex = 4;
		private const int typeIndex = 8;
		private const int bodyIndex = 12;
		private string _body;
		public RconPacket(PacketType type, string content)
		{
			if (type == null)
				throw new ArgumentException("Error: type parameter must not be null");

			Type = type;
			_body = content ?? string.Empty;
			Id = Environment.TickCount;
		}
		
		public int Size
		{
			get { return _body.Length + 10; }
		}
		public int Id { get; set; }
		public PacketType Type { get; internal set; }
		public string Body
		{
			get { return _body; }
		}

		public byte[] GetBytes()
		{
			byte[] buffer = new byte[Size + 4];
			Size.ToLittleEndian().CopyTo(buffer, sizeIndex);
			Id.ToLittleEndian().CopyTo(buffer, idIndex);
			Type.Value.ToLittleEndian().CopyTo(buffer, typeIndex);
			Encoding.UTF8.GetBytes(Body).CopyTo(buffer, bodyIndex);
			return buffer;
		}
	}
}
