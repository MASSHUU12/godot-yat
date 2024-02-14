using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("quickcommands", "Manages Quick Commands.", "[b]Usage[/b]: quickcommands [i]action[/i] [i]name[/i] [i]command[/i]", "qc")]
[Argument("action", "[add, remove, list]", "The action to perform.")]
[Option("-name", "string", "The name of the quick command.")]
[Option("-command", "string", "The command to execute when the quick command is called.")]
public sealed class QuickCommands : ICommand
{
	private BaseTerminal _terminal;
	private YAT _yat;

	public CommandResult Execute(CommandData data)
	{
		string action = (string)data.Arguments["action"];
		string name = data.Options.TryGetValue("-name", out object nameObj) ? (string)nameObj : null;
		string command = data.Options.TryGetValue("-command", out object commandObj) ? (string)commandObj : null;

		_yat = data.Yat;
		_terminal = data.Terminal;

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
				{
					_terminal.Print($"[b]{qc.Key}[/b] - {Text.EscapeBBCode(qc.Value)}");
				}
				break;
		}

		return ICommand.Success();
	}

	private CommandResult AddQuickCommand(string name, string command)
	{
		if (string.IsNullOrEmpty(command))
			return ICommand.Failure("You need to provide command for this action.");

		bool status = _yat.Commands.AddQuickCommand(name, command);
		string message;

		if (status) message = $"Added quick command '{name}'.";
		else message = $"Failed to add quick command '{name}'.";

		return status ? ICommand.Success(message) : ICommand.Failure(message);
	}

	private CommandResult RemoveQuickCommand(string name)
	{
		bool status = _yat.Commands.RemoveQuickCommand(name);
		string message;

		if (status) message = $"Removed quick command '{name}'.";
		else message = $"Failed to remove quick command '{name}'.";

		return status ? ICommand.Success(message) : ICommand.Failure(message);
	}
}
