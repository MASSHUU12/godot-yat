namespace YAT.Commands
{
	[Command("pause", "Toggles the game pause state.", "[b]Usage[/b]: pause")]
	public partial class Pause : ICommand
	{
		public CommandResult Execute(YAT yat, params string[] args)
		{
			yat.GetTree().Paused = !yat.GetTree().Paused;

			return CommandResult.Success;
		}
	}
}
