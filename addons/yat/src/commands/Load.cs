using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("load", "Loads specified object into the scene.", "[b]Usage[/b]: load [i]object_path[/i]")]
[Argument("object_path", "string", "The object path to load.")]
[Option("-absolute", null, "If true, the object will be loaded at the origin, otherwise relative positioning will be used.", false)]
[Option("-x", "float", "The X position of the object.", 0f)]
[Option("-y", "float", "The Y position of the object.", 0f)]
[Option("-z", "float", "The Z position of the object.", -5f)]
[Option("-rx", "float", "The X rotation of the object.", 0f)]
[Option("-ry", "float", "The Y rotation of the object.", 0f)]
[Option("-rz", "float", "The Z rotation of the object.", 0f)]
[Option("-sx", "float", "The X scale of the object.", 1f)]
[Option("-sy", "float", "The Y scale of the object.", 1f)]
[Option("-sz", "float", "The Z scale of the object.", 1f)]
[Option("-hidden", null, "If true, the object will be hidden.", true)]
public sealed class Load : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var path = (string)data.Arguments["object_path"];
		var absolute = (bool)data.Options["-absolute"];
		var position = new Vector3(
			(float)data.Options["-x"],
			(float)data.Options["-y"],
			(float)data.Options["-z"]
		);
		var rotation = new Vector3(
			(float)data.Options["-rx"],
			(float)data.Options["-ry"],
			(float)data.Options["-rz"]
		);
		var scale = new Vector3(
			(float)data.Options["-sx"],
			(float)data.Options["-sy"],
			(float)data.Options["-sz"]
		);
		var hidden = (bool)data.Options["-hidden"];
		var camera = data.Yat.GetViewport().GetCamera3D();

		if (!ValidatePath(path)) return ICommand.Failure("Invalid object path.");
		if (camera is null) return ICommand.Failure("No 3D camera found.");

		var scene = GD.Load<PackedScene>(path).Instantiate() as Node3D;

		data.Yat.GetTree().Root.AddChild(scene);

		TransformNode(scene, camera, position, rotation, scale, hidden, absolute);

		return ICommand.Success($"Object {scene.Name} loaded.");
	}

	private static void TransformNode(Node3D node, Camera3D camera, Vector3 position, Vector3 rotation, Vector3 scale, bool hidden, bool absolute)
	{
		if (absolute) node.GlobalPosition = position;
		else node.GlobalPosition = camera.GlobalTransform.Origin + camera.GlobalTransform.Basis.Z * position.Z;

		node.GlobalRotation = rotation;
		node.Scale = scale;
		node.Visible = !hidden;
	}

	private static bool ValidatePath(string path)
	{
		if (string.IsNullOrEmpty(path)) return false;
		if (!ResourceLoader.Exists(path)) return false;

		return true;
	}
}
