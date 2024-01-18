using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("quit", "By default quits the game.", "[b]Usage[/b]: quit", "exit")]
	[Option("-t", null, "Closes the terminal.", false)]
	public partial class Quit : ICommand
	{
		public CommandResult Execute(CommandData args)
		{
			var t = (bool)args.ConvertedArgs["-t"];

			if (t) CloseTerminal(args.Yat);
			else QuitTheGame(args.Yat);

			return CommandResult.Success;
		}

		private static void CloseTerminal(YAT yat)
		{
			yat.CallDeferred("CloseTerminal");
		}

		private static void QuitTheGame(YAT yat)
		{
			yat.Terminal.Print("Quitting...");
			yat.GetTree().Quit();
		}
	}
}
