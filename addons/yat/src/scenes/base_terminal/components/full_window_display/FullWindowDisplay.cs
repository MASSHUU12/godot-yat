using Godot;
using YAT.Helpers;

namespace YAT.Scenes;

public partial class FullWindowDisplay : Control
{
    [Signal] public delegate void OpenedEventHandler();
    [Signal] public delegate void ClosedEventHandler();

#nullable disable
    public bool IsOpen => Visible;
    public RichTextLabel MainDisplay { get; private set; }
    private Label _helpLabel;
#nullable restore

    public override void _Ready()
    {
        MainDisplay = GetNode<RichTextLabel>("%MainDisplay");
        _helpLabel = GetNode<Label>("%HelpLabel");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Keybindings.TerminalCloseFullWindowDisplay))
        {
            Close();
        }
    }

    public void Open(string text)
    {
        GenerateHelp();

        MainDisplay.Clear();
        _ = MainDisplay.CallDeferred("append_text", text);
        Visible = true;

        _ = EmitSignal(SignalName.Opened);
    }

    public void Close()
    {
        if (!Visible)
        {
            return;
        }

        MainDisplay.Clear();
        Visible = false;

        _ = EmitSignal(SignalName.Closed);
    }

    private void GenerateHelp()
    {
        InputEvent closeKey = InputMap.ActionGetEvents(
            Keybindings.TerminalCloseFullWindowDisplay
        )[0];

        _helpLabel.Text = $"{closeKey.AsText()} - Close";
    }
}
