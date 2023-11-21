using Godot;
using YAT.Overlay.Components.Terminal;

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
