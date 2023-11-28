using Godot;
using Godot.Collections;

namespace YAT.Scenes.Overlay.Components.Terminal
{
	public partial class QuickCommands : Resource
	{
		[Export] public Dictionary<string, string> Commands { get; set; } = new();
	}
}
