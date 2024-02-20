using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("whereami", "Prints the current scene name and path.", "[b]Usage[/b]: whereami", "wai")]
[Option("-l", "bool", "Prints the full path to the scene file.", false)]
[Option("-s", "bool", "Prints info about currently selected node.", false)]
public sealed class Whereami : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var scene = data.Yat.GetTree().CurrentScene;
		var longForm = (bool)data.Options["-l"];
		var s = (bool)data.Options["-s"];

		scene = s ? data.Terminal.SelectedNode.Current : scene;

		data.Terminal.Print(
			scene.GetPath() +
			(longForm ? " (" + scene.SceneFilePath + ")" : string.Empty)
		);

		return ICommand.Success();
	}
}
