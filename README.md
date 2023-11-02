<div align="center">
	<img src="./yat_icon_baner.png" />
	<h3>YAT</h1>
	<p>YAT is an addon that provides a customizable, in-game terminal for your project.</p>
</div>

YAT stands for Yet Another Terminal, which allows you to add your own commands to cheat, debug, or anything else. This add-on allows you to create custom commands with support for displaying windows.

> [!NOTE]
> This project is in the early stages of development. Many things may be added, removed or changed.
>
> However, the basic functions work properly and are usable.

## Prerequisites

-   [.NET SDK 7](https://dotnet.microsoft.com/en-us/download)
-   [.NET enabled Godot](https://godotengine.org/download/windows/)

## Usage

### Keybindings

To use this extension, you need to create these keybindings in your project:

-   `yat_toggle` - Toggles the state of the overlay.
-   `yat_history_previous` - Displays the previous command from history.
-   `yat_history_next` - Displays the next command from history.

### Commands

#### Builtin commands

> More information about each command can be found in their manuals.

| Command  | Alias  | Description                                                                      |
| -------- | ------ | -------------------------------------------------------------------------------- |
| cls      | clear  | Clears the console.                                                              |
| man      | N/A    | Displays the manual for a command.                                               |
| quit     | N/A    | Quits the game.                                                                  |
| echo     | N/A    | Displays the given text.                                                         |
| restart  | reboot | Restarts the level.                                                              |
| options  | N/A    | Creates the options window.                                                      |
| pause    | N/A    | Toggles the game pause state.                                                    |
| whereami | wai    | Prints the current scene name and path.                                          |
| list     | ls     | List all available commands.                                                     |
| view     | -      | Changes the rendering mode of the viewport.                                      |
| set      | -      | Sets a variable to a value. Does nothing by default, requires adding extensions. |

#### Creating commands

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

#### Adding commands

To add a command to the YAT all you have to do is call `AddCommand` method on YAT instance:

```csharp
GetNode<YAT>("/root/YAT").AddCommand(new Cls());
```

#### Making commands extendable

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

#### Extending commands

To be able to extend a command, you first need to create an extension,
which is a class that implements the `IExtension` interface,
and contains the `Extension` attribute, the rest works just like a regular command.

You can find an example of such a class in [example](./example) folder.

#### Creating custom windows

In progress.

### Signals

| Name            | Arguments             | Description                                 |
| --------------- | --------------------- | ------------------------------------------- |
| CloseRequested  | N/A                   | Sent when user wants to close custom window |
| OptionsChanged  | YatOptions            | Sent when YAT options have changed          |
| OverlayOpened   | N/A                   | Sent when overlay has been opened           |
| OverlayClosed   | N/A                   | Sent when overlay has been closed           |
| YatReady        | N/A                   | Sent when YAT is ready                      |
| CommandExecuted | command, args, result | Sent when the command was executed          |

## License

Licensed under [MIT license](./LICENSE).
