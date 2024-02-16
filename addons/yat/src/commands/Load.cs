using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("load", "Loads specified object into the scene.", "[b]Usage[/b]: load [i]object_path[/i]")]
[Argument("object_path", "string", "The object path to load.")]
public sealed class Load : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var path = data.Arguments["object_path"] as string;
		var camera = data.Yat.GetViewport().GetCamera3D();

		if (camera is null) return ICommand.Failure("No 3D camera found.");

		var scene = GD.Load<PackedScene>(path).Instantiate() as Node3D;

		data.Yat.GetTree().Root.AddChild(scene);

		scene.GlobalPosition = camera.GlobalTransform.Origin + camera.GlobalTransform.Basis.Z * 2;

		return ICommand.Success($"Object ${scene.Name} loaded.");
	}
}
