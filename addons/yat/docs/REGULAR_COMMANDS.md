<div align="center">
	<h3>Regular commands</h1>
	<p>Here you can find information about creating regular commands.</p>
</div>

> [!NOTE]
> General information on creating commands can be found [here](./CREATING_COMMANDS.md).

### Creating commands

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

### Command execution

The execution of the command begins in the `Execute` method.

YAT supports two types of `Execute` method for regular commands:

```csharp
public CommandResult Execute(params string[] args)
```

Basic version, passes raw arguments, works with and without automatic validation.

```csharp
public CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
```

It works similarly to the basic version with the difference that one of the arguments is a dictionary with the arguments converted to the appropriate types.
