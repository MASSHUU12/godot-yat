<div align="center">
	<h3>YAT</h1>
	<p>YAT is an addon that provides a customizable, in-game terminal for your project.</p>
</div>

YAT stands for Yet Another Terminal, which allows you to add your own commands to cheat, debug, or anything else. This add-on allows you to create custom commands with support for displaying windows.

## Prerequisites

-   [.NET SDK 7](https://dotnet.microsoft.com/en-us/download)
-   [.NET enabled Godot](https://godotengine.org/download/windows/)

## Usage

### Keybindings

To use this extension, you need to create these keybindings in your project:

-   `yat_toggle` - Toggles the state of the overlay.
-   `yat_history_previous` - Displays the previous command from history.
-   `yat_history_next` - Displays the next command from history.

### Builtin commands

> More information about each command can be found in their manuals.

| Command            | Alias  | Description                             |
| ------------------ | ------ | --------------------------------------- |
| cls                | clear  | Clears the console.                     |
| man <command_name> | N/A    | Displays the manual for a command.      |
| quit               | N/A    | Quits the game.                         |
| echo <text>        | N/A    | Displays the given text.                |
| restart            | reboot | Restarts the level.                     |
| options            | N/A    | Creates the options window.             |
| pause              | N/A    | Toggles the game pause state.           |
| whereami           | wai    | Prints the current scene name and path. |

### Creating commands

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

#### Signals

| Name            | Arguments             | Description                                 |
| --------------- | --------------------- | ------------------------------------------- |
| CloseRequested  | N/A                   | Sent when user wants to close custom window |
| OptionsChanged  | YatOptions            | Sent when YAT options have changed          |
| OverlayOpened   | N/A                   | Sent when overlay has been opened           |
| OverlayClosed   | N/A                   | Sent when overlay has been closed           |
| YatReady        | N/A                   | Sent when YAT is ready                      |
| CommandExecuted | command, args, result | Sent when the command was executed          |

#### Adding commands

To add a command to the YAT all you have to do is call `AddCommand` method on YAT instance:

```csharp
GetNode<YAT>("/root/YAT").AddCommand(new Cls());
```

#### Creating custom windows

Lorem ipsum dolor sit amet.

## License

Licensed under [MIT license](./LICENSE).
