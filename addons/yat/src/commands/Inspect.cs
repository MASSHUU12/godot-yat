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
[Option("-ray", "bool", "Use raycast to select an object.")]
public sealed class Inspect : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var useRayCast = (bool)data.Options["-ray"];

		if (useRayCast)
		{
			var result = InspectRayCastedObject(World.RayCast(data.Yat.GetViewport()));
			data.Terminal.Print(result);
		}
		else
		{
			var result = InspectNode(data.Terminal.SelectedNode.Current);
			data.Terminal.Print(result);
		}

		return ICommand.Success();
	}

	private static StringBuilder InspectRayCastedObject(Dictionary result)
	{
		StringBuilder sb = new();

		if (result is null || result.Count == 0)
		{
			sb.AppendLine("[b]No object found![/b]");
			return sb;
		}

		Node collider = result["collider"].As<Node>();

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

	private static StringBuilder InspectNode(Node node)
	{
		StringBuilder sb = new();

		if (node is null) return sb;

		sb.AppendLine();
		sb.AppendLine("[b]Node[/b]: " + node.Name);
		sb.AppendLine("[b]Path[/b]: " + node.GetPath());
		sb.AppendLine("[b]Scene Path[/b]: " + node.SceneFilePath);
		sb.AppendLine("[b]Script[/b]: " + node.GetScript());
		sb.AppendLine("[b]Type[/b]: " + node.GetType());
		sb.AppendLine("[b]Owner[/b]: " + node.Owner);
		sb.AppendLine("[b]Groups[/b]: " + string.Join(", ", node.GetGroups()));

		return sb;
	}
}
