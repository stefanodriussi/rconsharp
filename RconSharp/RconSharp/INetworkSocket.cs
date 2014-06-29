
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

using System.Threading.Tasks;
namespace RconSharp
{
	/// <summary>
	/// Shared interface for the Network Socket
	/// </summary>
	/// <remarks>Since this is a Portable Class Library (PCL), concrete implementations of this inteface will be defined in specific assemblies targeting the different platforms</remarks>
	public interface INetworkSocket
	{
		/// <summary>
		/// Connect the socket to the remote endpoint
		/// </summary>
		/// <returns>True if the connection was successfully; False if the connection is already estabilished</returns>
		Task<bool> ConnectAsync();
		/// <summary>
		/// Close the connection to the remote endpoint
		/// </summary>
		void CloseConnection();
		/// <summary>
		/// Send a Rcon command to the remote server
		/// </summary>
		/// <param name="data">Rcon command data</param>
		/// <returns>The response to the command sent</returns>
		Task<byte[]> SendDataAndReadResponseAsync(byte[] data);
		/// <summary>
		/// Gets whether the connection is opened or not
		/// </summary>
		bool IsConnected { get; }
	}
}
