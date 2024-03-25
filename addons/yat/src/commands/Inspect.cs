using System.Text;
using Godot;
using Godot.Collections;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("inspect", "Inspect selected object.", aliases: new[] { "ins" })]
[Usage("inspect [i]object[/i]")]
[Option("-ray", "bool", "Use RayCast to select an object.")]
[Option("-all", "bool", "Inspect all properties. Some properties might not be displayed correctly.")]
public sealed class Inspect : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var useRayCast = (bool)data.Options["-ray"];
		var all = (bool)data.Options["-all"];
		StringBuilder result;

		if (useRayCast) result = InspectRayCastedObject(World.RayCast(data.Yat.GetViewport()), all);
		else result = InspectNode(data.Terminal.SelectedNode.Current, all);

		return ICommand.Ok(result.ToString());
	}

	private static StringBuilder GetAllProperties(Node node)
	{
		StringBuilder sb = new();

		if (node is null) return sb;

		var properties = node.GetPropertyList();

		foreach (var property in properties)
		{
			var name = property["name"];
			var type = (int)property["type"];
			var propertyData = node.Get((string)name);

			sb.Append($"[b]{name}[/b]: ");

			if (type == (int)Variant.Type.Nil) sb.AppendLine();

			if (type == (int)Variant.Type.Vector2 || type == (int)Variant.Type.Vector2I)
				sb.AppendLine(propertyData.AsVector2().ToString());
			else if (type == (int)Variant.Type.Vector3 || type == (int)Variant.Type.Vector3I)
				sb.AppendLine(propertyData.AsVector3().ToString());
			else if (type == (int)Variant.Type.Vector4 || type == (int)Variant.Type.Vector4I)
				sb.AppendLine(propertyData.AsVector4().ToString());
			else if (type == (int)Variant.Type.Array)
				sb.AppendLine(string.Join(", ", propertyData.AsStringArray()));
			else if (type == (int)Variant.Type.Object)
			{
				var obj = propertyData.As<Script>();

				sb.AppendLine(obj?.ResourcePath);
			}
			else sb.AppendLine(propertyData.AsString());
		}

		return sb;
	}

	private static StringBuilder InspectRayCastedObject(Dictionary? result, bool all)
	{
		StringBuilder sb = new();

		if (result is null || result.Count == 0)
		{
			sb.AppendLine("[b]No object found.[/b]");
			return sb;
		}

		Node collider = result["collider"].As<Node>();

		if (all)
		{
			sb.Append(GetAllProperties(collider));
			return sb;
		}

		sb.AppendLine(InspectNode(collider).ToString());
		sb.AppendLine("[b]Collider[/b]: " + collider);
		sb.AppendLine("[b]ID[/b]: " + result["collider_id"]);
		sb.AppendLine("[b]Position[/b]: " + result["position"]);
		sb.AppendLine("[b]Normal[/b]: " + result["normal"]);
		sb.AppendLine("[b]Face Index[/b]: " + result["face_index"]);
		sb.AppendLine("[b]Shape[/b]: " + result["shape"]);
		sb.AppendLine("[b]RID[/b]: " + result["rid"]);

		return sb;
	}

	private static StringBuilder InspectNode(Node node, bool all = false)
	{
		StringBuilder sb = new();

		if (node is null) return sb;
		if (!GodotObject.IsInstanceValid(node))
		{
			sb.AppendLine("[b]Object is not valid.[/b]");
			return sb;
		}

		if (all)
		{
			sb.Append(GetAllProperties(node));
			return sb;
		}

		sb.AppendLine();
		sb.AppendLine("[b]Node[/b]: " + node.Name);
		sb.AppendLine("[b]UID[/b]: " + ResourceUid.IdToText(ResourceLoader.GetResourceUid(node.SceneFilePath)));
		sb.AppendLine("[b]Path[/b]: " + node.GetPath());
		sb.AppendLine("[b]Scene Path[/b]: " + node.SceneFilePath);
		sb.AppendLine("[b]Script[/b]: " + ((Script)node.GetScript())?.ResourcePath);
		sb.AppendLine("[b]Type[/b]: " + node.GetType());
		sb.AppendLine("[b]Owner[/b]: " + node.Owner);
		sb.AppendLine("[b]Groups[/b]: " + string.Join(", ", node.GetGroups()));

		if (node is Node2D node2D) sb.Append(InspectNode2D(node2D));
		else if (node is Node3D node3D) sb.Append(InspectNode3D(node3D));
		else if (node is Control control) sb.Append(InspectControl(control));

		return sb;
	}

	private static StringBuilder InspectNode3D(Node3D node)
	{
		StringBuilder sb = new();

		if (node is null) return sb;

		sb.AppendLine("[b]Transform[/b]: " + node.GlobalTransform);
		sb.AppendLine("[b]Rotation[/b]: " + node.RotationDegrees);
		sb.AppendLine("[b]Scale[/b]: " + node.Scale);
		sb.AppendLine("[b]Visible[/b]: " + node.Visible);

		return sb;
	}

	private static StringBuilder InspectNode2D(Node2D node)
	{
		StringBuilder sb = new();

		if (node is null) return sb;

		sb.AppendLine("[b]Transform[/b]: " + node.GlobalTransform);
		sb.AppendLine("[b]Rotation[/b]: " + node.RotationDegrees);
		sb.AppendLine("[b]Scale[/b]: " + node.Scale);
		sb.AppendLine("[b]Visible[/b]: " + node.Visible);

		return sb;
	}

	private static StringBuilder InspectControl(Control control)
	{
		StringBuilder sb = new();

		if (control is null) return sb;

		sb.AppendLine("[b]GlobalPosition[/b]: " + control.GlobalPosition);
		sb.AppendLine("[b]Rotation[/b]: " + control.Rotation);
		sb.AppendLine("[b]Size[/b]: " + control.Size);
		sb.AppendLine("[b]Rect[/b]: " + control.GetRect());

		return sb;
	}
}
