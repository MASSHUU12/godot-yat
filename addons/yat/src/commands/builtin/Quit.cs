using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("quit", "Quits the game.", "[b]Usage[/b]: quit", "exit")]
	public partial class Quit : ICommand
	{
		public CommandResult Execute(CommandArguments args)
		{
			args.Terminal.Print("Quitting...");
			args.Yat.GetTree().Quit();

			return CommandResult.Success;
		}
	}
}
