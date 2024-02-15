using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("cs", "Changes the scene.", "[b]Usage[/b]: cs [i]scene[/i]")]
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
		var oldPath = data.Yat.GetTree().CurrentScene.SceneFilePath;
		var scene = (string)data.Arguments["scene"];

		if (!ResourceLoader.Exists(scene, typeof(PackedScene).Name))
		{
			EmitSignal(SignalName.SceneChangeFailed, scene, (short)ESceneChangeFailureReason.DoesNotExist);

			return ICommand.Failure($"Scene '{scene}' does not exist.");
		}

		EmitSignal(SignalName.SceneAboutToChange, oldPath, scene);

		var error = data.Yat.GetTree().ChangeSceneToFile(scene);

		if (error != Error.Ok)
		{
			EmitSignal(SignalName.SceneChangeFailed, scene, (short)(
				error == Error.CantOpen
				? ESceneChangeFailureReason.CantOpen
				: ESceneChangeFailureReason.CantInstantiate
			));

			return ICommand.Failure($"Failed to change scene to '{scene}'.");
		}

		EmitSignal(SignalName.SceneChanged, scene);

		data.Terminal.Print($"Changed scene to '{scene}'.", EPrintType.Success);

		return ICommand.Success();
	}
}
