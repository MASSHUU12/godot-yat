using System;
using Godot;
using YAT.Helpers;
using YAT.Resources;

namespace YAT.Scenes;

public partial class GameTerminal : YatWindow
{
#nullable disable
    public BaseTerminal CurrentTerminal { get; private set; }
    public TerminalSwitcher TerminalSwitcher { get; private set; }
#nullable restore

    public override void _Ready()
    {
        base._Ready();

        TerminalSwitcher = GetNode<TerminalSwitcher>("%TerminalSwitcher");

        CurrentTerminal = TerminalSwitcher.CurrentTerminal;
        CurrentTerminal.TitleChangeRequested += title => Title = title;
        CurrentTerminal.PositionResetRequested += ResetPosition;
        CurrentTerminal.SizeResetRequested += () => Size = InitialSize;

        CloseRequested += _yat.TerminalManager.CloseTerminal;

#if GODOT4_3_OR_GREATER
        ContextMenu.AddSubmenuNodeItem(
            "QuickCommands",
            GetNode<QuickCommandsContext>("ContextMenu/QuickCommandsContext")
        );
#else
		ContextMenu.AddSubmenuItem("QuickCommands", "QuickCommandsContext");
#endif
        ContextMenu.AddItem("Preferences", 1);
        ContextMenu.IdPressed += ContextMenuItemSelected;

        MoveToCenter();
    }

    private void ContextMenuItemSelected(long id)
    {
        if (id == 1)
        {
            Commands.Preferences prefs = new();
            _ = prefs.Execute(new(
                _yat, CurrentTerminal, prefs, Array.Empty<string>(), new(), new(), default
            ));
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Keybindings.TerminalToggle))
        {
            _ = CallDeferred("emit_signal", SignalName.CloseRequested);
        }
    }

    private new void UpdateOptions(YatPreferences prefs)
    {
        Size = new(prefs.DefaultWidth, prefs.DefaultHeight);

        base.UpdateOptions(prefs);
    }
}
