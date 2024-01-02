using System.Collections.Generic;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes.Terminal;

namespace YAT.Commands
{
	[Command(
		"cn",
		"Changes the selected node to the specified node path.",
		"[b]Usage[/b]: cn [i]node_path[/i]"
	)]
	[Argument("node_path", "string", "The node path of the new selected node.")]
	public partial class Cn : ICommand
	{
		public YAT Yat { get; set; }

		public Cn(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
		{
			var path = cArgs["node_path"] as string;
			var result = Yat.Terminal.SelectedNode.ChangeSelectedNode(path);

			if (!result) Yat.Terminal.Print($"Invalid node path: {path}", Terminal.PrintType.Error);

			return result ? CommandResult.Success : CommandResult.Failure;
		}
	}
}
