using Godot;
using YAT.Helpers;

namespace YAT.Scenes;

public partial class TerminalManager : Node
{
    [Signal] public delegate void TerminalOpenedEventHandler();
    [Signal] public delegate void TerminalClosedEventHandler();

#nullable disable
    public GameTerminal GameTerminal;
    public BaseTerminal CurrentTerminal { get; private set; }

    private YAT _yat;
#nullable restore

    public override void _Ready()
    {
        _yat = GetNode<YAT>("/root/YAT");

        GameTerminal = GetNode<GameTerminal>("%GameTerminal");
        GameTerminal.Visible = false;
        GameTerminal.CloseRequested += CloseTerminal;
        GameTerminal.TerminalSwitcher.CurrentTerminalChanged += (terminal) =>
        {
            CurrentTerminal = terminal;
        };

        CurrentTerminal = GameTerminal.TerminalSwitcher.CurrentTerminal;
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
        CurrentTerminal.Input.Clear();

        _ = EmitSignal(SignalName.TerminalOpened);
    }

    public void CloseTerminal()
    {
        GameTerminal.Visible = false;

        _ = EmitSignal(SignalName.TerminalClosed);
    }
}
