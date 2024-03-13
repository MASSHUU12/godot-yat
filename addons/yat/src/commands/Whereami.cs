using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("whereami", "Prints the current scene name and path.", "[b]Usage[/b]: whereami", "wai")]
[Option("-l", "bool", "Prints the full path to the scene file.")]
[Option("-s", "bool", "Prints info about currently selected node.")]
[Option("-e", "bool", "Prints the path to the executable.")]
public sealed class Whereami : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var s = (bool)data.Options["-s"];
		var e = (bool)data.Options["-e"];
		var longForm = (bool)data.Options["-l"];

		return ICommand.Ok(
			e
			? GetExecutablePath(data.Terminal)
			: GetSceneInfo(data.Terminal, s, longForm)
		);
	}

	private static string GetExecutablePath(BaseTerminal terminal) =>
		OS.GetExecutablePath().GetBaseDir();

	private static string GetSceneInfo(BaseTerminal terminal, bool s, bool l)
	{
		var scene = terminal.GetTree().CurrentScene;
		scene = s ? terminal.SelectedNode.Current : scene;

		return string.Format(
			"{0} {1}",
			scene.GetPath(),
			l ? "(" + scene.SceneFilePath + ")" : string.Empty
		);
	}
}
