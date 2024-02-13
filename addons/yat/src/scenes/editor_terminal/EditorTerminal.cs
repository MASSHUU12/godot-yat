using Godot;

namespace YAT.Scenes;

[Tool]
public partial class EditorTerminal : Control
{
	public BaseTerminal BaseTerminal { get; private set; }

	public override void _Ready()
	{
		BaseTerminal = GetNode<BaseTerminal>("Content/BaseTerminal");
	}

	// public override void _Input(InputEvent @event)
	// {
	// 	if (@event.IsActionPressed("yat_toggle"))
	// 	{
	// 		CallDeferred("emit_signal", nameof(BaseTerminal.CloseRequested));
	// 	}
	// }
}
