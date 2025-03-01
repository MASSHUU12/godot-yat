using System.Globalization;
using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("whereami", "Prints the current scene name and path.", aliases: "wai")]
[Option("-l", "bool", "Prints the full path to the scene file.")]
[Option("-s", "bool", "Prints info about currently selected node.")]
[Option("-e", "bool", "Prints the path to the executable.")]
public sealed class Whereami : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        bool s = (bool)data.Options["-s"];
        bool e = (bool)data.Options["-e"];
        bool longForm = (bool)data.Options["-l"];

        string p = e
            ? GetExecutablePath()
            : GetSceneInfo(data.Terminal, s, longForm);
        return ICommand.Ok([p], p);
    }

    private static string GetExecutablePath()
    {
        return OS.GetExecutablePath().GetBaseDir();
    }

    private static string GetSceneInfo(BaseTerminal terminal, bool s, bool l)
    {
        Node scene = terminal.GetTree().CurrentScene;
        scene = s ? terminal.SelectedNode.Current : scene;

        return string.Format(
            CultureInfo.InvariantCulture,
            "{0} {1}",
            scene.GetPath(),
            l ? "(" + scene.SceneFilePath + ")" : string.Empty
        );
    }
}
