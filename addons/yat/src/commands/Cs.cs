using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("cs", "Changes the scene.")]
[Argument("scene", "string", "The scene to change to.")]
public partial class Cs : Node, ICommand
{
    [Signal]
    public delegate void SceneAboutToChangeEventHandler(string oldPath, string newPath);
    [Signal]
    public delegate void SceneChangedEventHandler(string scenePath);
    [Signal]
    public delegate void SceneChangeFailedEventHandler(string scenePath, ESceneChangeFailureReason reason);

    public CommandResult Execute(CommandData data)
    {
        var scene = (string)data.Arguments["scene"];

        if (!CheckSceneExistence(scene))
        {
            return ICommand.Failure(
                $"Scene '{scene}' does not exist."
            );
        }

        EmitSceneAboutToChange(data.Yat, scene);

        if (!ChangeScene(scene, data.Yat))
        {
            return ICommand.Failure(
                $"Failed to change scene to '{scene}'."
            );
        }

        _ = EmitSignal(SignalName.SceneChanged, scene);

        return ICommand.Success($"Changed scene to '{scene}'.");
    }

    private bool CheckSceneExistence(string scene)
    {
        var sceneExists = ResourceLoader.Exists(scene, nameof(PackedScene));

        if (!sceneExists)
        {
            _ = EmitSignal(
                SignalName.SceneChangeFailed,
                scene,
                (short)ESceneChangeFailureReason.DoesNotExist
            );
        }

        return sceneExists;
    }

    private void EmitSceneAboutToChange(YAT yat, string scene)
    {
        var oldPath = yat.GetTree().CurrentScene.SceneFilePath;
        _ = EmitSignal(SignalName.SceneAboutToChange, oldPath, scene);
    }

    private bool ChangeScene(string scene, YAT yat)
    {
        var error = yat.GetTree().ChangeSceneToFile(scene);
        if (error == Error.Ok)
        {
            return true;
        }

        _ = EmitSignal(SignalName.SceneChangeFailed, scene, (short)(
            error == Error.CantOpen
            ? ESceneChangeFailureReason.CantOpen
            : ESceneChangeFailureReason.CantInstantiate
        ));

        return false;
    }
}
