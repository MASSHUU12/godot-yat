using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes.BaseTerminal;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Commands
{
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
			{
				_terminal.Print("You need to provide a command name for this action.", PrintType.Error);
				return CommandResult.Failure;
			}

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

			return CommandResult.Success;
		}

		private CommandResult AddQuickCommand(string name, string command)
		{
			if (string.IsNullOrEmpty(command))
			{
				_terminal.Print("You need to provide command for this action.", PrintType.Error);
				return CommandResult.Failure;
			}

			bool status = _yat.Commands.AddQuickCommand(name, command);
			string message;

			if (status) message = $"Added quick command '{name}'.";
			else message = $"Failed to add quick command '{name}'.";

			_terminal.Print(message, status ? PrintType.Success : PrintType.Error);

			return status ? CommandResult.Success : CommandResult.Failure;
		}

		private CommandResult RemoveQuickCommand(string name)
		{
			bool status = _yat.Commands.RemoveQuickCommand(name);
			string message;

			if (status) message = $"Removed quick command '{name}'.";
			else message = $"Failed to remove quick command '{name}'.";

			_terminal.Print(message, status ? PrintType.Success : PrintType.Error);

			return status ? CommandResult.Success : CommandResult.Failure;
		}
	}
}
