using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes.BaseTerminal;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Commands
{
	[Command(
		"cn",
		"Changes the selected node to the specified node path.",
		"[b]Usage[/b]: cn [i]node_path[/i]"
	)]
	[Argument(
		"node_path",
		"string",
		"The node path of the new selected node.\n" +
		"Use [i]&[/i] to use the RayCast method to get the node path where the camera is looking at.\n" +
		"Use [i]&[b]ray_length[/b][/i] to specify the ray length (default 256).\n" +
		"Example: [i]&[/i] or [i]&[b]256[/b][/i]"
	)]
	public partial class Cn : ICommand
	{
		private const float DEFAULT_RAY_LENGTH = 256;
		private const char RAY_CAST_PREFIX = '&';

		private YAT _yat;
		private BaseTerminal _terminal;

		public CommandResult Execute(CommandArguments args)
		{
			var path = args.ConvertedArgs["node_path"] as string;
			bool result;

			_yat = args.Yat;
			_terminal = args.Terminal;

			// If the path starts with RAY_CAST_PREFIX use RayCast to get the node path
			// where the camera is looking at
			if (path.StartsWith(RAY_CAST_PREFIX)) result = ChangeSelectedNode(GetNodePath(path));
			else result = ChangeSelectedNode(path);

			if (!result) args.Terminal.Print($"Invalid node path: {path}", PrintType.Error);

			return result ? CommandResult.Success : CommandResult.Failure;
		}

		private NodePath GetNodePath(string path)
		{
			var result = World.RayCast(_yat.GetViewport(), GetRayLength(path));

			if (result is null)
			{
				_terminal.Print("No collider found.", PrintType.Error);
				return null;
			}

			Node collider = result.TryGetValue("collider", out Variant value) ? value.As<Node>() : null;

			return collider?.GetPath();
		}

		private static float GetRayLength(string path)
		{
			return NumericHelper.TryConvert(path[1..], out float rayLength)
				? rayLength
				: DEFAULT_RAY_LENGTH;
		}

		private bool ChangeSelectedNode(NodePath path)
		{
			return _terminal.SelectedNode.ChangeSelectedNode(path);
		}
	}
}
