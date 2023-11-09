namespace YAT.Commands
{
	[Command("quit", "Quits the game.", "[b]Usage[/b]: quit", "exit")]
	public partial class Quit : ICommand
	{
		public YAT Yat { get; set; }

		public Quit(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			Yat.Terminal.Print("Quitting...");
			Yat.GetTree().Quit();

			return CommandResult.Success;
		}
	}
}
