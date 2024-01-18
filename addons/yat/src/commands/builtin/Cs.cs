using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Commands
{
	[Command("cs", "Changes the scene.", "[b]Usage[/b]: cs [i]scene[/i]")]
	[Argument("scene", "string", "The scene to change to.")]
	public partial class Cs : Node, ICommand
	{
		[Signal]
		public delegate void SceneAboutToChangeEventHandler(string scenePath);
		[Signal]
		public delegate void SceneChangedEventHandler(string scenePath);
		[Signal]
		public delegate void SceneChangeFailedEventHandler(string scenePath, FailureReason reason);

		public enum FailureReason
		{
			SceneCantOpen,
			SceneDoesNotExist,
			SceneCantInstantiate,
		}

		public CommandResult Execute(CommandData data)
		{
			var scene = data.RawData[1];

			if (!ResourceLoader.Exists(scene, typeof(PackedScene).Name))
			{
				EmitSignal(SignalName.SceneChangeFailed, scene, (short)FailureReason.SceneDoesNotExist);

				data.Terminal.Print($"Scene '{scene}' does not exist.", PrintType.Error);

				return CommandResult.Failure;
			}

			EmitSignal(SignalName.SceneAboutToChange, scene);

			var error = data.Yat.GetTree().ChangeSceneToFile(scene);

			if (error != Error.Ok)
			{
				EmitSignal(SignalName.SceneChangeFailed, scene, (short)(
					error == Error.CantOpen
					? FailureReason.SceneCantOpen
					: FailureReason.SceneCantInstantiate
				));

				data.Terminal.Print($"Failed to change scene to '{scene}'.", PrintType.Error);

				return CommandResult.Failure;
			}

			EmitSignal(SignalName.SceneChanged, scene);

			data.Terminal.Print($"Changed scene to '{scene}'.", PrintType.Success);

			return CommandResult.Success;
		}
	}
}
