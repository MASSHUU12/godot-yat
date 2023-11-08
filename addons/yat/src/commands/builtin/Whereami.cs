namespace YAT.Commands
{
	[Command("whereami", "Prints the current scene name and path.", "[b]Usage[/b]: whereami", "wai")]
	public partial class Whereami : ICommand
	{
		public CommandResult Execute(YAT yat, params string[] args)
		{
			var scene = yat.GetTree().CurrentScene;

			yat.Terminal.Print($"{scene.GetPath()} ({scene.SceneFilePath})");

			return CommandResult.Success;
		}
	}
}