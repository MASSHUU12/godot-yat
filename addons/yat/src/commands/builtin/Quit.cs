using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("quit", "By default quits the game.", "[b]Usage[/b]: quit", "exit")]
	[Option("-t", null, "Closes the terminal.", false)]
	public partial class Quit : ICommand
	{
		public CommandResult Execute(CommandData data)
		{
			var t = (bool)data.Options["-t"];

			if (t) CloseTerminal(data.Yat);
			else QuitTheGame(data.Yat);

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
