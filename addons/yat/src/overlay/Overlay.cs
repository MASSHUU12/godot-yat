using Godot;

namespace YAT
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
