using System.Text;
using Godot;
using Godot.Collections;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("inspect", "Inspect selected object.", aliases: new[] { "ins" })]
[Option("-ray", "bool", "Use RayCast to select an object.")]
[Option("-all", "bool", "Inspect all properties. Some properties might not be displayed correctly.")]
public sealed class Inspect : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        var useRayCast = (bool)data.Options["-ray"];
        var all = (bool)data.Options["-all"];
        StringBuilder result = useRayCast
            ? InspectRayCastedObject(World.RayCast(data.Yat.GetViewport()), all)
            : InspectNode(data.Terminal.SelectedNode.Current, all);

        return ICommand.Ok(result.ToString());
    }

    private static StringBuilder GetAllProperties(Node node)
    {
        StringBuilder sb = new();

        if (node is null)
        {
            return sb;
        }

        foreach (var property in (Array<Dictionary>)node.GetPropertyList())
        {
            var name = property["name"];
            var type = (int)property["type"];
            var propertyData = node.Get((string)name);

            _ = sb.Append($"[b]{name}[/b]: ");

            if (type == (int)Variant.Type.Nil)
            {
                _ = sb.AppendLine();
            }

            if (type is ((int)Variant.Type.Vector2) or ((int)Variant.Type.Vector2I))
            {
                _ = sb.AppendLine(propertyData.AsVector2().ToString());
            }
            else if (type is ((int)Variant.Type.Vector3) or ((int)Variant.Type.Vector3I))
            {
                _ = sb.AppendLine(propertyData.AsVector3().ToString());
            }
            else if (type is ((int)Variant.Type.Vector4) or ((int)Variant.Type.Vector4I))
            {
                _ = sb.AppendLine(propertyData.AsVector4().ToString());
            }
            else if (type == (int)Variant.Type.Array)
            {
                _ = sb.AppendJoin(", ", propertyData.AsStringArray()).AppendLine();
            }
            else if (type == (int)Variant.Type.Object)
            {
                var obj = propertyData.As<Script>();

                _ = sb.AppendLine(obj?.ResourcePath);
            }
            else
            {
                _ = sb.AppendLine(propertyData.AsString());
            }
        }

        return sb;
    }

    private static StringBuilder InspectRayCastedObject(Dictionary? result, bool all)
    {
        StringBuilder sb = new();

        if (result is null || result.Count == 0)
        {
            _ = sb.AppendLine("[b]No object found.[/b]");
            return sb;
        }

        Node collider = result["collider"].As<Node>();

        if (all)
        {
            _ = sb.Append(GetAllProperties(collider));
            return sb;
        }

        _ = sb.AppendLine(InspectNode(collider).ToString())
            .AppendLine("[b]Collider[/b]: " + collider)
            .AppendLine("[b]ID[/b]: " + result["collider_id"])
            .AppendLine("[b]Position[/b]: " + result["position"])
            .AppendLine("[b]Normal[/b]: " + result["normal"])
            .AppendLine("[b]Face Index[/b]: " + result["face_index"])
            .AppendLine("[b]Shape[/b]: " + result["shape"])
            .AppendLine("[b]RID[/b]: " + result["rid"]);

        return sb;
    }

    private static StringBuilder InspectNode(Node node, bool all = false)
    {
        StringBuilder sb = new();

        if (node is null)
        {
            return sb;
        }

        if (!GodotObject.IsInstanceValid(node))
        {
            _ = sb.AppendLine("[b]Object is not valid.[/b]");
            return sb;
        }

        if (all)
        {
            _ = sb.Append(GetAllProperties(node));
            return sb;
        }

        _ = sb.AppendLine()
            .AppendLine("[b]Node[/b]: " + node.Name)
            .AppendLine("[b]UID[/b]: " + ResourceUid.IdToText(ResourceLoader.GetResourceUid(node.SceneFilePath)))
            .AppendLine("[b]Path[/b]: " + node.GetPath())
            .AppendLine("[b]Scene Path[/b]: " + node.SceneFilePath)
            .AppendLine("[b]Script[/b]: " + ((Script)node.GetScript())?.ResourcePath)
            .AppendLine("[b]Type[/b]: " + node.GetType())
            .AppendLine("[b]Owner[/b]: " + node.Owner)
            .AppendLine("[b]Groups[/b]: " + string.Join(", ", node.GetGroups()));

        if (node is Node2D node2D)
        {
            _ = sb.Append(InspectNode2D(node2D));
        }
        else if (node is Node3D node3D)
        {
            _ = sb.Append(InspectNode3D(node3D));
        }
        else if (node is Control control)
        {
            _ = sb.Append(InspectControl(control));
        }

        return sb;
    }

    private static StringBuilder InspectNode3D(Node3D node)
    {
        StringBuilder sb = new();

        if (node is null)
        {
            return sb;
        }

        _ = sb.AppendLine("[b]Transform[/b]: " + node.GlobalTransform)
            .AppendLine("[b]Rotation[/b]: " + node.RotationDegrees)
            .AppendLine("[b]Scale[/b]: " + node.Scale)
            .AppendLine("[b]Visible[/b]: " + node.Visible);

        return sb;
    }

    private static StringBuilder InspectNode2D(Node2D node)
    {
        StringBuilder sb = new();

        if (node is null)
        {
            return sb;
        }

        _ = sb.AppendLine("[b]Transform[/b]: " + node.GlobalTransform)
            .AppendLine("[b]Rotation[/b]: " + node.RotationDegrees)
            .AppendLine("[b]Scale[/b]: " + node.Scale)
            .AppendLine("[b]Visible[/b]: " + node.Visible);

        return sb;
    }

    private static StringBuilder InspectControl(Control control)
    {
        StringBuilder sb = new();

        if (control is null)
        {
            return sb;
        }

        _ = sb.AppendLine("[b]GlobalPosition[/b]: " + control.GlobalPosition)
            .AppendLine("[b]Rotation[/b]: " + control.Rotation)
            .AppendLine("[b]Size[/b]: " + control.Size)
            .AppendLine("[b]Rect[/b]: " + control.GetRect());

        return sb;
    }
}
