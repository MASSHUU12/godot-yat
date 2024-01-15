using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes.BaseTerminal;

namespace YAT.Commands
{
	[Command(
		"sys",
		"Runs a system command.",
		"[b]sys[/b] [i]command[/i]"
	)]
	[Argument(
		"command",
		"string",
		"The command to run (if contains more than one word, you need to wrap it in the parentheses)."
	)]
	[Option("-program", "string", "The program to run the command with (default to systems specific terminal).", "")]
	public sealed class Sys : ICommand
	{
		private BaseTerminal _terminal;

		public CommandResult Execute(CommandArguments args)
		{
			var program = (string)args.ConvertedArgs["-program"];
			var command = (string)args.ConvertedArgs["command"];
			var commandName = command.Split(' ')[0];
			var commandArgs = command[commandName.Length..].Trim() ?? string.Empty;

			_terminal = args.Terminal;

			OS.RunCommand(commandName, program, commandArgs);

			return CommandResult.Success;
		}
	}
}
