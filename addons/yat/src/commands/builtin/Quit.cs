namespace YAT.Commands
{
	[Command("quit", "Quits the game.", "[b]Usage[/b]: quit", "exit")]
	public partial class Quit : ICommand
	{
		public CommandResult Execute(YAT yat, params string[] args)
		{
			yat.Terminal.Print("Quitting...");
			yat.GetTree().Quit();

			return CommandResult.Success;
		}
	}
}
