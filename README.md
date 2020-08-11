# rconsharp

rconsharp is a [Valve RCON](https://developer.valvesoftware.com/wiki/Source_RCON_Protocol) protocol implementation for `netstandard`

## Support

In theory all servers claiming to implement Valve's RCON protocol should be supported.
In reality there are subtle differences in the way many developer chooses to interpret the protocol, leading to some unexpected result here and there.
The library has been tested successfully with:
* CS:GO
* Minecraft
* ARK: Survival Evolved

If you stumble upon a game that doesn't work for some reason, please fill an issue and I'll do my best to see what's wrong.

## Build

The easiest way to build rconsharp is by cloning the repo, open the solution file in Visual Studio and then build the proeject via `Build` menu
Alternatively `dotnet CLI` can be used to achieve the same result (both Windows and Unix environments are supported)
```
dotnet restore
dotnet build --configuration Release --no-restore
```

### Tests

A xUnit test project is included in this repository and covers most of the `rconsharp` API (feel free to submit a PR to improve coverage).
All tests can be executed using Visual Studio runner with a nice report of all the results.
As for the build process, the same result can be achieved using `dotnet CLI`
```
dotnet restore
dotnet build --configuration Release --no-restore
dotnet test --no-restore --verbosity normal
```
## NuGet packages

A package is available via NuGet. Search for `RconSharp` via Visual Studio dependencies manager window or use NuGet Packet Manager CLI

```
PM> Install-Package rconsharp
```

## Samples

### Authenticate and send a command

This is a very basic snippet that allows you to forward commands to a Rcon enabled remote server.

```
// Create an instance of RconClient pointing to an IP and a PORT
var client = RconClient.Create("127.0.0.1", 15348);

// Send a RCON packet with type AUTH and the RCON password for the target server
var authenticated = await client.AuthenticateAsync("RCONPASSWORD");
if (authenticated)
{
    // If the response is positive, the connection is authenticated and further commands can be sent
    var status = await client.ExecuteCommandAsync("status");
    // Some responses will be split into multiple RCON pakcets when body length exceeds the maximum allowed
    // For this reason these commands needs to be issued with isMultiPacketResponse parameter set to true
    // An example is CS:GO cvarlist
    var cvarlist = await client.ExecuteCommandAsync("cvarlist", true);
}
```

Please note that while `AuthenticateAsync` method call will always return `true` or `false`, a call to `ExecuteCommandAsync` could raise a `TaskCanceledException` in the event of a connection loss or timeout. You should surround the requesting code with a `try .. catch` block to properly handle these situations.

## Updating from v1

The previous version of `netstandard` was built as a PCL and contained platform specific implementations for the actual `Socket` communication. The new version is an almost complete rewrite of the whole library and backward compatibility has not been preserved, sorry about that.
The good news is that the API has been simplified, resulting in a fairly easy port. 

## Licensing

This code is distributed under the very permissive MIT License but, if you use it, you might consider referring to the repository. Please refer to [LICENSE](./LICENSE) file for the complete license description.
