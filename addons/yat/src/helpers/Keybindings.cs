using System;
using Godot;

namespace YAT.Helpers;

public static class Keybindings
{
    public static readonly StringName ContextMenu = new("yat_context_menu");
    public static readonly StringName TerminalToggle = new("yat_terminal_toggle");
    public static readonly StringName TerminalInterrupt = new("yat_terminal_interrupt");
    public static readonly StringName TerminalHistoryNext = new("yat_terminal_history_next");
    public static readonly StringName TerminalHistoryPrevious = new("yat_terminal_history_previous");
    public static readonly StringName TerminalAutocompletionNext = new("yat_terminal_autocompletion_next");
    public static readonly StringName TerminalAutocompletionPrevious = new("yat_terminal_autocompletion_previous");
    public static readonly StringName TerminalCloseFullWindowDisplay = new("yat_terminal_close_full_window_display");

    public static readonly StringName ExamplePlayerMoveLeft = new("yat_example_player_move_left");
    public static readonly StringName ExamplePlayerMoveRight = new("yat_example_player_move_right");
    public static readonly StringName ExamplePlayerMoveForward = new("yat_example_player_move_forward");
    public static readonly StringName ExamplePlayerMoveBackward = new("yat_example_player_move_backward");

    private static readonly Tuple<StringName, InputEvent>[] _defaultActions = new Tuple<StringName, InputEvent>[]
    {
        new(TerminalToggle, new InputEventKey { PhysicalKeycode = Key.Quoteleft }),
        new(ContextMenu, new InputEventMouseButton {
            ButtonMask = MouseButtonMask.Right,
            ButtonIndex = MouseButton.Right
        }),
        new(TerminalHistoryPrevious, new InputEventKey { PhysicalKeycode = Key.Up }),
        new(TerminalHistoryNext, new InputEventKey { PhysicalKeycode = Key.Down }),
        new(TerminalInterrupt, new InputEventKey {
            PhysicalKeycode = Key.C,
            CtrlPressed = true
        }),
        new(TerminalAutocompletionNext, new InputEventKey { PhysicalKeycode = Key.Tab }),
        new(TerminalAutocompletionPrevious, new InputEventKey {
            PhysicalKeycode = Key.Tab,
            ShiftPressed = true
        }),
        new(TerminalCloseFullWindowDisplay, new InputEventKey { PhysicalKeycode = Key.Q }),
        new(ExamplePlayerMoveLeft, new InputEventKey { PhysicalKeycode = Key.A }),
        new(ExamplePlayerMoveRight, new InputEventKey { PhysicalKeycode = Key.D }),
        new(ExamplePlayerMoveForward, new InputEventKey { PhysicalKeycode = Key.W }),
        new(ExamplePlayerMoveBackward, new InputEventKey { PhysicalKeycode = Key.S }),
    };

    public static void LoadDefaultActions()
    {
        foreach (Tuple<StringName, InputEvent> action in _defaultActions)
        {
            if (InputMap.HasAction(action.Item1))
            {
                continue;
            }

            InputMap.AddAction(action.Item1);
            InputMap.ActionAddEvent(action.Item1, action.Item2);
        }
    }
}
