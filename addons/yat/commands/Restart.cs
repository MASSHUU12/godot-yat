namespace YAT.Commands
{
	[Command("restart", "Restarts the level.", "[b]Usage[/b]: restart", "reboot")]
	public partial class Restart : ICommand
	{
		public CommandResult Execute(YAT yat, params string[] args)
		{
			yat.Terminal.Println($"Restarting {yat.GetTree().CurrentScene.Name}...");
			yat.GetTree().ReloadCurrentScene();

			return CommandResult.Success;
		}
	}
}
