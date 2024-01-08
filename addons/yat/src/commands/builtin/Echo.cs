using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("echo", "Displays the given text.", "[b]Usage[/b]: echo [i]text[/i]")]
	[Argument("message", "string", "The text to display.")]
	public partial class Echo : ICommand
	{
		public CommandResult Execute(CommandArguments args)
		{
			var text = string.Join(" ", args.Arguments[1..^0]);
			args.Terminal.Print(text);

			return CommandResult.Success;
		}
	}
}
