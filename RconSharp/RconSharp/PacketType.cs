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

namespace RconSharp
{
	/// <summary>
	/// RCON Packet type
	/// </summary>
	public class PacketType
	{
		/// <summary>
		/// Gets the value according to RCON protocol
		/// </summary>
		public int Value { get; internal set; }

		/// <summary>
		/// Gets the original RCON Packet Type name as defined in the protocol
		/// </summary>
		public string ProtocolName { get; internal set; }

		/// <summary>
		/// Auth Packet Type
		/// </summary>
		public static PacketType Auth = new PacketType() { ProtocolName = "SERVERDATA_AUTH", Value = 3 };

		/// <summary>
		/// Auth Response Packet Type
		/// </summary>
		public static PacketType AuthResponse = new PacketType() { ProtocolName = "SERVERDATA_AUTH_RESPONSE", Value = 2 };

		/// <summary>
		/// Exec Command Packet Type
		/// </summary>
		public static PacketType ExecCommand = new PacketType() { ProtocolName = "SERVERDATA_EXECCOMMAND", Value = 2 };

		/// <summary>
		/// Response Value Packet Type
		/// </summary>
		public static PacketType ResponseValue = new PacketType() { ProtocolName = "SERVERDATA_RESPONSE_VALUE", Value = 0 };
	}
}
