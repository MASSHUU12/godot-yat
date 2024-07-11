using Godot;
using YAT.Helpers;

namespace YAT.Scenes;

public partial class TerminalManager : Node
{
    [Signal] public delegate void TerminalOpenedEventHandler();
    [Signal] public delegate void TerminalClosedEventHandler();

#nullable disable
    public GameTerminal GameTerminal;

    private YAT _yat;
#nullable restore

    public override void _Ready()
    {
        GameTerminal = GD.Load<PackedScene>("uid://dsyqv187j7w76").Instantiate<GameTerminal>();

        _yat = GetNode<YAT>("/root/YAT");
        _yat.Ready += () =>
        {
            AddChild(GameTerminal);
            GameTerminal.Visible = false;
        };
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed(Keybindings.TerminalToggle))
        {
            ToggleTerminal();
        }
    }

    public void ToggleTerminal()
    {
        if (!_yat.YatEnable.YatEnabled)
        {
            return;
        }

        if (GameTerminal.Visible)
        {
            CloseTerminal();
        }
        else
        {
            OpenTerminal();
        }
    }

    public void OpenTerminal()
    {
        GameTerminal.Visible = true;
        // 'Prevents' writing to the input when the terminal is toggled
        _yat.CurrentTerminal.Input.Clear();

        _ = EmitSignal(SignalName.TerminalOpened);
    }

    public void CloseTerminal()
    {
        GameTerminal.Visible = false;

        _ = EmitSignal(SignalName.TerminalClosed);
    }
}
