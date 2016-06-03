# rconsharp

rconsharp is a [Valve RCON](https://developer.valvesoftware.com/wiki/Source_RCON_Protocol) protocol implementation for .NET written in C# as a Portable Class Library (PCL).

## Usage

Clone this repository and build the solution to get the assemblies. At this point of time, you can either reference the PCL and provide your custom implementation of `INetworkSocket` or reference the other projects (right now only .Net 4.5 is implemented).
Due to the shared structure of PCLs it's not possible to have a common implementation of a network socket and this is the reason why a concrete class to handle network communication will be needed for each platform you need to target.

### NuGet packages

Instead of manually downloading binaries or building the sources, you can simply add it by installing the NuGet package. Just search for `rconsharp` from the package manager or type `PM> Install-Package RconSharp` in NuGet command line.

There's also a package contining an implementation of `INetworkSocket` interface. Search for `rconsharp.socket` or type `PM> Install-Package RconSharp.Socket ` in NuGet command line.
Currently only .net 4.5 is supported and as soon as i can find some spare time i will provide implementations for other platforms.

## Quick example

This is a very basic snippet that allows you to forward commands to a Rcon enabled remote server.
(In order to run this example, you also need the package `RconSharp.Socket` to be installed from NuGet)

```
// create an instance of the socket. In this case i've used the .Net 4.5 object defined in the project
INetworkSocket socket = new RconSocket();

// create the RconMessenger instance and inject the socket
RconMessenger messenger = new RconMessenger(socket);

// initiate the connection with the remote server
bool isConnected = await messenger.ConnectAsync("remotehost", 12345);

// try to authenticate with your supersecretpassword (... obviously this is my hackerproof key, you shoul use yours)
bool authenticated = await messenger.AuthenticateAsync("supersecretpassword");
if (authenticated)
{
  // if we fall here, we're good to go! from this point on the connection is authenticated and you can send commands 
  // to the server
  var response = await messenger.ExecuteCommandAsync("/help");
}
```

Note: rconsharp is designed to work with the `async/await` paradigm. Nothing to be afraid of but remember you have to mark the encapsulating method with the `async` keyword and you have to `await` the `awaitable` methods in order to get the results. Refer to the [official documentation](http://msdn.microsoft.com/en-us/library/hh191443.aspx) if you wish to learn more on the subject.

### Dependencies

All the dependencies within this project are referenced as NuGet packages and will be restored upon first build (if you have this option enabled NuGet settings).
Following is the list of referenced packages:
* Microsoft Async
* Microsoft BCL Build Components
* Microsoft BCL Portability Pack
* Moq (just for the tests)


## Client example

The minecraft client example has been moved to his own repository. You can find it [here](https://github.com/stefanodriussi/minecraft-remote-controller)

## Licensing

This code is distributed under the very permissive MIT License but, if you use it, you might consider referring to the repository. Please refer to [LICENSE](./LICENSE) file for the complete license description.
