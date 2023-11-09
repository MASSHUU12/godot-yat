namespace YAT.Commands
{
	[Command("restart", "Restarts the level.", "[b]Usage[/b]: restart", "reboot")]
	public partial class Restart : ICommand
	{
		public YAT Yat { get; set; }

		public Restart(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			Yat.Terminal.Print($"Restarting {Yat.GetTree().CurrentScene.Name}...");
			Yat.GetTree().ReloadCurrentScene();

			return CommandResult.Success;
		}
	}
}
