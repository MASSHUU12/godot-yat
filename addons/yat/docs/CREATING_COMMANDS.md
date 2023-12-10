<div align="center">
	<h3>Creating commands</h1>
	<p>Here you can find information on creating new commands, extending existing commands, and adding them to YAT</p>
</div>

## Table of Contents

- [Table of Contents](#table-of-contents)
- [Creating commands](#creating-commands)
	- [Threaded commands](#threaded-commands)
	- [Overridable methods](#overridable-methods)
		- [GenerateCommandManual](#generatecommandmanual)
- [Adding commands](#adding-commands)
- [Making commands extendable](#making-commands-extendable)
	- [Overridable methods](#overridable-methods-1)
		- [GenerateExtensionsManual](#generateextensionsmanual)
- [Extending commands](#extending-commands)
	- [Overridable methods](#overridable-methods-2)
		- [GenerateExtensionManual](#generateextensionmanual)
- [Creating custom windows](#creating-custom-windows)
- [Signals](#signals)

## Creating commands

To create a command, you need to create C# file and implement `ICommand` interface.

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

Depending on your needs, YAT supports 4 types of `Execute` method that you can use:

```csharp
public CommandResult Execute(params string[] args)
```

Basic version, you can use it when you want to run a command on the main thread, both without and with automatic validation.

```csharp
public CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
```

Similar to the basic version, it runs on the main thread, but it takes as the first argument data that has passed validation and been converted to the appropriate types.

```csharp
public CommandResult Execute(CancellationToken ct, params string[] args)
```

It is equivalent to the first version, but runs on a separate thread, and accepts a CancellationToken.

```csharp
public CommandResult Execute(Dictionary<string, object> cArgs, CancellationToken ct, params string[] args)
```

It is equivalent to the second version, but runs on a separate thread, and accepts a CancellationToken.

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

| Name            | Arguments             | Description                                          |
| --------------- | --------------------- | ---------------------------------------------------- |
| OptionsChanged  | YatOptions            | Sent when YAT options have changed                   |
| YatReady        | N/A                   | Sent when YAT is ready                               |
| CommandStarted  | command, args         | Signal emitted when a command execution has started. |
| CommandFinished | command, args, result | Signal emitted when a command has been executed      |
