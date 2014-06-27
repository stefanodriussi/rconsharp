using System;
using System.Threading.Tasks;

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
	/// Rcon protocol messages handler
	/// </summary>
	public class RconMessenger
	{
		private INetworkSocket _socket;

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="socket">Socket interface</param>
		/// <remarks>Since this is a Portable Class Library, there's not a concrete implementation of the Socket class</remarks>
		public RconMessenger(INetworkSocket socket)
		{
			_socket = socket;
		}

		/// <summary>
		/// Send the proper authentication packet and parse the response
		/// </summary>
		/// <param name="password">Current server password</param>
		/// <returns>True if the connection has been authenticated; False elsewhere</returns>
		public async Task<bool> Authenticate(string password)
		{
			throw new NotImplementedException();
		}
	}
}
