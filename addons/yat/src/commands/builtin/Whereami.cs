using YAT.Attributes;

namespace YAT.Commands
{
	[Command("whereami", "Prints the current scene name and path.", "[b]Usage[/b]: whereami", "wai")]
	public partial class Whereami : ICommand
	{
		public YAT Yat { get; set; }

		public Whereami(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			var scene = Yat.GetTree().CurrentScene;

			Yat.Terminal.Print($"{scene.GetPath()} ({scene.SceneFilePath})");

			return CommandResult.Success;
		}
	}
}
