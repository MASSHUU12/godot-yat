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

		public YAT Yat { get; set; }

		public Cs(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			var scene = args[1];

			if (!ResourceLoader.Exists(scene, typeof(PackedScene).Name))
			{
				EmitSignal(SignalName.SceneChangeFailed, scene, (short)FailureReason.SceneDoesNotExist);

				Yat.Terminal.Print($"Scene '{scene}' does not exist.", PrintType.Error);

				return CommandResult.Failure;
			}

			EmitSignal(SignalName.SceneAboutToChange, scene);

			var error = Yat.GetTree().ChangeSceneToFile(scene);

			if (error != Error.Ok)
			{
				EmitSignal(SignalName.SceneChangeFailed, scene, (short)(
					error == Error.CantOpen
					? FailureReason.SceneCantOpen
					: FailureReason.SceneCantInstantiate
				));

				Yat.Terminal.Print($"Failed to change scene to '{scene}'.", PrintType.Error);

				return CommandResult.Failure;
			}

			EmitSignal(SignalName.SceneChanged, scene);

			Yat.Terminal.Print($"Changed scene to '{scene}'.", PrintType.Success);

			return CommandResult.Success;
		}
	}
}
