using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("quickcommands", aliases: "qc")]
[Description("Manages Quick Commands.")]
[Usage("qc [i]action[/i]")]
[Argument("action", "add|remove|list", "The action to perform.")]
[Option("-name", "string", "The name of the quick command.")]
[Option("-command", "string", "The command to execute when the quick command is called.")]
public sealed class QuickCommands : ICommand
{
#nullable disable
	private YAT _yat;
#nullable restore

	public CommandResult Execute(CommandData data)
	{
		var action = (string)data.Arguments["action"];
		var name = (string)data.Options["-name"];
		var command = (string)data.Options["-command"];

		_yat = data.Yat;

		if (action != "list" && string.IsNullOrEmpty(name))
			return ICommand.Failure("You need to provide a command name for this action.");

		switch (action)
		{
			case "add":
				return AddQuickCommand(name, command);
			case "remove":
				return RemoveQuickCommand(name);
			default:
				foreach (var qc in _yat.Commands.QuickCommands.Commands)
					data.Terminal.Print($"[b]{qc.Key}[/b] - {Text.EscapeBBCode(qc.Value)}");
				break;
		}

		return ICommand.Success();
	}

	private CommandResult AddQuickCommand(string name, string command)
	{
		if (string.IsNullOrEmpty(command))
			return ICommand.Failure("You need to provide command for this action.");

		return _yat.Commands.AddQuickCommand(name, command)
			? ICommand.Success($"Added quick command '{name}'.")
			: ICommand.Failure($"Failed to add quick command '{name}'.");
	}

	private CommandResult RemoveQuickCommand(string name)
	{
		return _yat.Commands.RemoveQuickCommand(name)
			? ICommand.Success($"Removed quick command '{name}'.")
			: ICommand.Failure($"Failed to remove quick command '{name}'.");
	}
}
