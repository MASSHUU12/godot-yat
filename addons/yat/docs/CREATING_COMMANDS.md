<div align="center">
	<h3>Creating commands</h1>
	<p>Here you can find information on creating new commands, extending existing commands, and adding them to YAT</p>
</div>

## Table of Contents

-   [Table of Contents](#table-of-contents)
-   [Creating commands](#creating-commands)
    -   [Threaded commands](#threaded-commands)
    -   [Overridable methods](#overridable-methods)
        -   [GenerateCommandManual](#generatecommandmanual)
-   [Adding commands](#adding-commands)
-   [Making commands extendable](#making-commands-extendable)
    -   [Overridable methods](#overridable-methods-1)
        -   [GenerateExtensionsManual](#generateextensionsmanual)
-   [Extending commands](#extending-commands)
    -   [Overridable methods](#overridable-methods-2)
        -   [GenerateExtensionManual](#generateextensionmanual)
-   [Creating custom windows](#creating-custom-windows)
-   [Signals](#signals)

## Creating commands

To create a command, you need to create C# file and implement `IYatCommand` interface.

In addition, you must use the `Command` attribute to add the necessary metadata for the command.

The `Command` attribute accepts the command `name`, its `description`, `manual` and `aliases`. The description and manual have BBCode support.

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
	public partial class Cls : ICommand
	{
		public YAT Yat { get; set; }

		public Cls(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			Yat.Terminal.Clear();

			return CommandResult.Success;
		}
	}
}
```

### Threaded commands

By default, commands are run on the `main thread`, which is not a problem for simple commands, but can quickly become cumbersome for more time-consuming tasks.

> [!IMPORTANT]
> Using engine features via a command running on a separate thread can be problematic.
>
> Fortunately, rather most errors can be circumvented by using methods:
> CallDeferred, CallThreadSafe or CallDeferredThreadGroup.

To run the command in a separate thread, you must use the `Threaded` attribute.

In addition, you must use another overload of the `Execute` method, which takes a `CancellationToken`:

```csharp
public virtual CommandResult Execute(CancellationToken ct, params string[] args)
```

It is important to make proper use of the `CancellationToken`, as it is the one that indicates when the method should `end early`.

### Overridable methods

#### GenerateCommandManual

This method generates command documentation based on the metadata added via the `Command` attribute, but nothing prevents you from overriding it and creating your own logic.

## Adding commands

To add a command to the YAT all you have to do is call `AddCommand` method on YAT instance:

```csharp
var yat = GetNode<YAT>("/root/YAT");
yat.AddCommand(new Cls(yat));
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

This method generates extension documentation based on the metadata added via the `Extension` attribute, but nothing prevents you from overriding it and creating your own logic.

## Creating custom windows

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
