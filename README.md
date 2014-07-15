# rconsharp

rconsharp is a [Valve RCON](https://developer.valvesoftware.com/wiki/Source_RCON_Protocol) protocol implementation for .NET written in C# as a Portable Class Library (PCL).

## Usage

Clone this repository and build the solution to get the assemblies. At this point of time, you can either reference the PCL and provide your custom implementation of `INetworkSocket` or reference the other projects (right now only .Net 4.5 is implemented).
Due to the shared structure of PCLs it's not possible to have a common implementation of a network socket and this is the reason why a concrete class to handle network communication will be needed for each platform you need to target.

## Quick example

This is a very basic snippet that allows you to forward commands to a Rcon enabled remote server.

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
  var response = messenger.ExecuteCommandAsync("/help");
}
```

Note: rconsharp is designed to work with the `async/await` paradigm. Nothing to be afraid of but remember you have to mark the encapsulating method with the `async` keyword. Refer to the [official documentation](http://msdn.microsoft.com/en-us/library/hh191443.aspx) if you wish to learn more on the subject.

### Dependencies

All the dependencies within this project are referenced as NuGet packages and will be restored upon first build (if you have this option enabled NuGet settings).
Following is the list of referenced packages:
* Microsoft Async
* Microsoft BCL Build Components
* Microsoft BCL Portability Pack
* Mvvm Light (for the test client)
* Moq (just for the tests)


## Client example

I've created a simple client for real world testing purposes. It's developed using WPF so technically it's Windows only (maybe i'll create another version more OS independent). As the name suggests, it's really simple: connects to a RCON Minecraft Server (but actually could be any RCON enabled server if you can stand the pixeled theme), sends commands and reads responses.
It can be found under `SimpleRconClient` path.

![Minecraft RCON Client screenshot](/assets/screenshot.png)

## Licensing

This code is distributed under the very permissive MIT License but, if you use it, you might consider referring to the repository. Please refer to [LICENSE](./LICENSE) file for the complete license description.
