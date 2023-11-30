using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes.Overlay.Components.Terminal;

namespace YAT.Commands
{
	[Command("quickcommands", "Manages Quick Commands.", "[b]Usage[/b]: quickcommands [i]action[/i] [i]name[/i] [i]command[/i]", "qc")]
	[Argument("action", "[add, remove, list]", "The action to perform.")]
	[Option("-name", "string", "The name of the quick command.")]
	[Option("-command", "string", "The command to execute when the quick command is called.")]
	public sealed class QuickCommands : ICommand
	{
		public YAT Yat { get; set; }

		public QuickCommands(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(System.Collections.Generic.Dictionary<string, object> cArgs, params string[] args)
		{
			string action = (string)cArgs["action"];
			string name = cArgs.TryGetValue("-name", out object nameObj) ? (string)nameObj : null;
			string command = cArgs.TryGetValue("-command", out object commandObj) ? (string)commandObj : null;

			if (action != "list" && string.IsNullOrEmpty(name))
			{
				Yat.Terminal.Print("You need to provide a command name for this action.", Terminal.PrintType.Error);
				return CommandResult.Failure;
			}

			switch (action)
			{
				case "add":
					return AddQuickCommand(name, command);
				case "remove":
					return RemoveQuickCommand(name);
				default:
					foreach (var qc in Yat.Terminal.Context.QuickCommands.QuickCommands.Commands)
					{
						Yat.Terminal.Print($"[b]{qc.Key}[/b] - {TextHelper.EscapeBBCode(qc.Value)}");
					}
					break;
			}

			return CommandResult.Success;
		}

		private CommandResult AddQuickCommand(string name, string command)
		{
			if (string.IsNullOrEmpty(command))
			{
				Yat.Terminal.Print("You need to provide command for this action.", Terminal.PrintType.Error);
				return CommandResult.Failure;
			}

			bool status = Yat.Terminal.Context.QuickCommands.AddQuickCommand(name, command);
			string message;

			if (status) message = $"Added quick command '{name}'.";
			else message = $"Failed to add quick command '{name}'.";

			Yat.Terminal.Print(message, status ? Terminal.PrintType.Success : Terminal.PrintType.Error);

			return status ? CommandResult.Success : CommandResult.Failure;
		}

		private CommandResult RemoveQuickCommand(string name)
		{
			bool status = Yat.Terminal.Context.QuickCommands.RemoveQuickCommand(name);
			string message;

			if (status) message = $"Removed quick command '{name}'.";
			else message = $"Failed to remove quick command '{name}'.";

			Yat.Terminal.Print(message, status ? Terminal.PrintType.Success : Terminal.PrintType.Error);

			return status ? CommandResult.Success : CommandResult.Failure;
		}
	}
}
