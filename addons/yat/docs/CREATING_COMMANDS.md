<div align="center">
	<h3>Creating commands</h1>
	<p>Here you can find information on creating new commands, extending existing commands, and adding them to YAT</p>
</div>

## Creating commands

Information about automatic input validation can be found [here](./AUTOMATIC_INPUT_VALIDATION.md).

To create a command, you need to create C# file and implement `ICommand` interface.

In addition, you must use the `Command` attribute to add the necessary metadata for the command.

The `Command` attribute accepts the command `name`, its `description`, `manual` and `aliases`.
The description and manual have BBCode support.

The execution of the command begins in the `Execute` method.
The `Execute` method accepts `CommandData`, which contains probably all the things your command could ever need, these are things like: reference to YAT and BaseTerminal, raw arguments & options, converted arguments & options, cancellation token and more.

As an example, let's look at Cls command:

```csharp
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command(
		"cls",
		"Clears the console.",
		"[b]Usage[/b]: cls",
		"clear"
	)]
	public sealed class Cls : ICommand
	{
		public CommandResult Execute(CommandData data)
		{
			data.Terminal.Clear();

			return CommandResult.Success;
		}
	}
}
```

### Threaded commands

> [!IMPORTANT]
> Using engine features via a command running on a separate thread can be problematic.
>
> Fortunately, rather most errors can be circumvented by using:
> CallDeferred, CallThreadSafe or CallDeferredThreadGroup.

Creating a command that runs on a separate thread looks very similar to creating a regular command.

Therefore, first create a regular command and then add a `Threaded` attribute to it, which allows it to run on a separate thread.

In the passed `CommandData` you can find the `CancellationToken` which indicates when the command was asked to terminate prematurely.

### Overridable methods

-   GenerateCommandManual
-   GenerateArgumentsManual
-   GenerateOptionsManual
-   GenerateSignalsManual

## Adding commands

To add a command to the YAT all you have to do is call `AddCommand` method on `CommandManager` instance:

```csharp
var yat = GetNode<YAT>("/root/YAT");
yat.CommandManager.AddCommand(new Cls());
```

## Making commands extendable

It is possible to extend existing commands under several conditions:

1. an existing command must inherit from the class `Extensible`.
2. The command must call the `Execute` method on a specific extension. Example below.

```csharp
var variable = args[1];

if (Extensions.ContainsKey(variable))
{
   var extension = Extensions[variable];
   return extension.Execute(yat, this, args[1..]);
}
```

### Overridable methods

#### GenerateExtensionsManual

This method creates documentation for all extensions of a command. It does this by running the method `GenerateExtensionManual` on each extension.

## Extending commands

To be able to extend a command, you first need to create an extension,
which is a class that implements the `IExtension` interface,
and contains the `Extension` attribute, the rest works just like a regular command.

You can find an example of such a class in [example](./example) folder.

### Overridable methods

#### GenerateExtensionManual

This method generates extension documentation based on the metadata added via the `Extension` attribute.

## Signals

| Name            | Arguments             | Description                                          |
| --------------- | --------------------- | ---------------------------------------------------- |
| OptionsChanged  | YatOptions            | Sent when YAT options have changed                   |
| YatReady        | N/A                   | Sent when YAT is ready                               |
| CommandStarted  | command, args         | Signal emitted when a command execution has started. |
| CommandFinished | command, args, result | Signal emitted when a command has been executed      |
| TerminalOpened  | N/A                   | Emitted when terminal is opened.                     |
| TerminalClosed  | N/A                   | Emitted when terminal is closed.                     |
