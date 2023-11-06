<div align="center">
	<h3>Creating commands</h1>
	<p>Here you can find information on creating new commands, and adding them to YAT.</p>
</div>

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
