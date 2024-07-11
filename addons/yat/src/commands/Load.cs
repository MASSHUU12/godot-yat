using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("load", "Loads specified object into the scene.")]
[Argument("object_path", "string", "The object path to load.")]
[Option("-absolute", "bool", "If true, the object will be loaded at the origin, otherwise relative positioning will be used.")]
[Option("-x", "float", "The X position of the object.", 0f)]
[Option("-y", "float", "The Y position of the object.", 0f)]
[Option("-z", "float", "The Z position of the object.", -5f)]
[Option("-rx", "float", "The X rotation of the object.", 0f)]
[Option("-ry", "float", "The Y rotation of the object.", 0f)]
[Option("-rz", "float", "The Z rotation of the object.", 0f)]
[Option("-sx", "float", "The X scale of the object.", 1f)]
[Option("-sy", "float", "The Y scale of the object.", 1f)]
[Option("-sz", "float", "The Z scale of the object.", 1f)]
[Option("-hidden", "bool", "The object will be hidden.")]
[Option("-2d", "bool", "The object will be loaded as a 2D object.")]
public sealed class Load : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        var path = (string)data.Arguments["object_path"];
        var is2D = (bool)data.Options["-2d"];

        if (!ValidatePath(path))
        {
            return ICommand.Failure("Invalid object path.");
        }

        return !is2D ? LoadNode3D(path, data) : LoadNode2D(path, data);
    }

    private static CommandResult LoadNode3D(string path, CommandData data)
    {
        var camera = data.Yat.GetViewport().GetCamera3D();
        var absolute = (bool)data.Options["-absolute"];
        Vector3 position = new(
            (float)data.Options["-x"],
            (float)data.Options["-y"],
            (float)data.Options["-z"]
        );
        Vector3 rotation = new(
            (float)data.Options["-rx"],
            (float)data.Options["-ry"],
            (float)data.Options["-rz"]
        );
        Vector3 scale = new(
            (float)data.Options["-sx"],
            (float)data.Options["-sy"],
            (float)data.Options["-sz"]
        );
        var hidden = (bool)data.Options["-hidden"];

        if (camera is null && !absolute)
        {
            return ICommand.Failure("No 3D camera found.");
        }

        if (GD.Load<PackedScene>(path).Instantiate() is not Node3D node)
        {
            return ICommand.Failure("Failed to load object.");
        }

        data.Yat.GetTree().Root.AddChild(node);

        TransformNode3D(node, camera!, position, rotation, scale, hidden, absolute);

        return ICommand.Success(
            string.Format(
                "Object '{0}' loaded at {1} with rotation {2} and scale {3}.",
                node.Name,
                node.Position,
                node.Rotation,
                node.Scale
            )
        );
    }

    private static CommandResult LoadNode2D(string path, CommandData data)
    {
        var camera = data.Yat.GetViewport().GetCamera2D();
        var absolute = (bool)data.Options["-absolute"];
        Vector2 position = new(
            (float)data.Options["-x"],
            (float)data.Options["-y"]
        );
        var rotation = (float)data.Options["-rz"];
        Vector2 scale = new(
            (float)data.Options["-sx"],
            (float)data.Options["-sy"]
        );
        var hidden = (bool)data.Options["-hidden"];

        if (camera is null && !absolute)
        {
            return ICommand.Failure("No 2D camera found.");
        }

        if (GD.Load<PackedScene>(path).Instantiate() is not Node2D node)
        {
            return ICommand.Failure("Failed to load object.");
        }

        data.Yat.GetTree().Root.AddChild(node);

        TransformNode2D(node, camera!, position, rotation, scale, hidden, absolute);

        return ICommand.Success(
            string.Format(
                "Object '{0}' loaded at {1} with rotation {2} and scale {3}.",
                node.Name,
                node.Position,
                node.Rotation,
                node.Scale
            )
        );
    }

    private static void TransformNode3D(
        Node3D node,
        Camera3D camera,
        Vector3 position,
        Vector3 rotation,
        Vector3 scale,
        bool hidden,
        bool absolute
    )
    {
        node.GlobalPosition = absolute
            ? position
            : camera.GlobalTransform.Origin
                + (camera.GlobalTransform.Basis * position);

        node.GlobalRotation = rotation;
        node.Scale = scale;
        node.Visible = !hidden;
    }

    private static void TransformNode2D(
        Node2D node,
        Camera2D camera,
        Vector2 position,
        float rotation,
        Vector2 scale,
        bool hidden,
        bool absolute
    )
    {
        if (absolute)
        {
            node.GlobalPosition = position;
        }
        else
        {
            node.GlobalPosition = camera.GlobalTransform.Origin
                                  + (camera.GlobalTransform.X * position.X)
                                  + (camera.GlobalTransform.Y * position.Y);
        }

        node.GlobalRotation = rotation;
        node.Scale = scale;
        node.Visible = !hidden;
    }

    private static bool ValidatePath(string path)
    {
        return !string.IsNullOrEmpty(path) && ResourceLoader.Exists(path);
    }
}
