using Godot;

namespace YAT
{
	public partial class YatOverlay : Control
	{
		public Terminal Terminal;

		private YAT _yat;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("..");
			Terminal = GD.Load<PackedScene>(
				"res://addons/yat/yat_overlay/components/terminal/Terminal.tscn"
			).Instantiate<Terminal>();
		}

		public override void _Input(InputEvent @event)
		{
			if (@event.IsActionPressed("yat_toggle"))
			{
				// Toggle the CLI window.
				if (Terminal.IsInsideTree())
				{
					Terminal.Input.ReleaseFocus();
					RemoveChild(Terminal);
				}
				else
				{
					AddChild(Terminal);
					// Grabbing focus this way prevents writing to the input field
					// from the previous frame.
					Terminal.Input.CallDeferred("grab_focus");
				}
			}
		}
	}
}
