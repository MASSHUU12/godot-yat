using YAT.Attributes;
using YAT.Enums;
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
	public sealed class Sys : ICommand
	{
		private BaseTerminal _terminal;

		public CommandResult Execute(CommandArguments args)
		{
			_terminal = args.Terminal;

			RunSystemCommand((string)args.ConvertedArgs["command"]);

			return CommandResult.Success;
		}

		private void RunSystemCommand(string args)
		{

		}
	}
}
