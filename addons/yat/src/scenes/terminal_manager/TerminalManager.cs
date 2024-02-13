using Godot;
using YAT.Helpers;

namespace YAT.Scenes;

public partial class TerminalManager : Node
{
	[Signal] public delegate void TerminalOpenedEventHandler();
	[Signal] public delegate void TerminalClosedEventHandler();

	public GameTerminal GameTerminal;

	private YAT _yat;

	public override void _Ready()
	{
		_yat = GetNode<YAT>("/root/YAT");

		GameTerminal = GD.Load<PackedScene>("uid://dsyqv187j7w76").Instantiate<GameTerminal>();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed(Keybindings.TerminalToggle)) ToggleTerminal();
	}

	public void ToggleTerminal()
	{
		if (!_yat.YatEnabled) return;

		if (GameTerminal.IsInsideTree()) CloseTerminal();
		else OpenTerminal();
	}

	public void OpenTerminal()
	{
		if (GameTerminal.IsInsideTree()) return;

		AddChild(GameTerminal);

		EmitSignal(SignalName.TerminalOpened);
	}

	public void CloseTerminal()
	{
		if (!GameTerminal.IsInsideTree()) return;

		RemoveChild(GameTerminal);

		EmitSignal(SignalName.TerminalClosed);
	}
}
