using Godot;
using YAT.Scenes.Overlay.Components.Terminal;

namespace YAT.Scenes.Overlay
{
	public partial class Overlay : Control
	{
		public Terminal Terminal;

		public override void _Ready()
		{
			Terminal = GetNode<Terminal>("Terminal");
		}
	}
}
