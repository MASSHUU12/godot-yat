using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("whereami", "Prints the current scene name and path.", "[b]Usage[/b]: whereami", "wai")]
	[Option("-l", null, "Prints the full path to the scene file.", false)]
	[Option("-s", null, "Prints info about currently selected node.", false)]
	public partial class Whereami : ICommand
	{
		public CommandResult Execute(CommandArguments args)
		{
			var scene = args.Yat.GetTree().CurrentScene;
			var longForm = (bool)args.ConvertedArgs["-l"];
			var s = (bool)args.ConvertedArgs["-s"];

			scene = s ? args.Terminal.SelectedNode.Current : scene;

			args.Terminal.Print(
				scene.GetPath() +
				(longForm ? " (" + scene.SceneFilePath + ")" : string.Empty)
			);

			return CommandResult.Success;
		}
	}
}
