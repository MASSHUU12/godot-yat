using System.Collections.Generic;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("whereami", "Prints the current scene name and path.", "[b]Usage[/b]: whereami", "wai")]
	[Option("-l", null, "Prints the full path to the scene file.", false)]
	[Option("-s", null, "Prints info about currently selected node.", false)]
	public partial class Whereami : ICommand
	{
		public YAT Yat { get; set; }

		public Whereami(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
		{
			var scene = Yat.GetTree().CurrentScene;
			var longForm = (bool)cArgs["-l"];
			var s = (bool)cArgs["-s"];

			scene = s ? Yat.Terminal.SelectedNode.Current : scene;

			Yat.Terminal.Print(
				scene.GetPath() +
				(longForm ? " (" + scene.SceneFilePath + ")" : string.Empty)
			);

			return CommandResult.Success;
		}
	}
}
