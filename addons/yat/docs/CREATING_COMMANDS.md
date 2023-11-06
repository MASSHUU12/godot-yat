<div align="center">
	<h3>Creating commands</h1>
	<p>Here you can find information on creating new commands, extending existing commands, and adding them to YAT</p>
</div>

## Table of Contents

-   [Table of Contents](#table-of-contents)
-   [Creating commands](#creating-commands)
-   [Adding commands](#adding-commands)
-   [Making commands extendable](#making-commands-extendable)
-   [Extending commands](#extending-commands)
-   [Signals](#signals)

## Creating commands

To create a command, you need to create C# file and implement `IYatCommand` interface.

In addition, you must use the `Command` attribute to add the necessary metadata for the command.

The `Command` attribute accepts the command `name`, its `description`, `manual` and `aliases`. The description and manual have BBCode support.

As an example, let's look at `Cls` command:

```csharp
[Command(
	"cls",
	"Clears the console.",
	"[b]Usage[/b]: cls",
	"clear"
)]
public partial class Cls : ICommand
{
	public CommandResult Execute(YAT yat, params string[] args)
	{
		yat.Terminal.Clear();

		return CommandResult.Success;
	}
}
```

## Adding commands

To add a command to the YAT all you have to do is call `AddCommand` method on YAT instance:

```csharp
GetNode<YAT>("/root/YAT").AddCommand(new Cls());
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

## Extending commands

To be able to extend a command, you first need to create an extension,
which is a class that implements the `IExtension` interface,
and contains the `Extension` attribute, the rest works just like a regular command.

You can find an example of such a class in [example](./example) folder.

###Creating custom windows

In progress.

## Signals

| Name            | Arguments             | Description                                 |
| --------------- | --------------------- | ------------------------------------------- |
| CloseRequested  | N/A                   | Sent when user wants to close custom window |
| OptionsChanged  | YatOptions            | Sent when YAT options have changed          |
| OverlayOpened   | N/A                   | Sent when overlay has been opened           |
| OverlayClosed   | N/A                   | Sent when overlay has been closed           |
| YatReady        | N/A                   | Sent when YAT is ready                      |
| CommandExecuted | command, args, result | Sent when the command was executed          |
