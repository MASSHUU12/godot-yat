using System;
using Godot;

namespace YAT.Helpers;

public static class Keybindings
{
    public static readonly string ContextMenu = "yat_context_menu";
    public static readonly string TerminalToggle = "yat_terminal_toggle";
    public static readonly string TerminalInterrupt = "yat_terminal_interrupt";
    public static readonly string TerminalHistoryNext = "yat_terminal_history_next";
    public static readonly string TerminalHistoryPrevious = "yat_terminal_history_previous";
    public static readonly string TerminalAutocompletionNext = "yat_terminal_autocompletion_next";
    public static readonly string TerminalAutocompletionPrevious = "yat_terminal_autocompletion_previous";
    public static readonly string TerminalCloseFullWindowDisplay = "yat_terminal_close_full_window_display";

    public static readonly string ExamplePlayerMoveLeft = "yat_example_player_move_left";
    public static readonly string ExamplePlayerMoveRight = "yat_example_player_move_right";
    public static readonly string ExamplePlayerMoveForward = "yat_example_player_move_forward";
    public static readonly string ExamplePlayerMoveBackward = "yat_example_player_move_backward";

    private static readonly Tuple<string, InputEvent>[] _defaultActions = new Tuple<string, InputEvent>[]
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
        foreach (Tuple<string, InputEvent> action in _defaultActions)
        {
            if (InputMap.Singleton.HasAction(action.Item1))
            {
                continue;
            }

            InputMap.Singleton.AddAction(action.Item1);
            InputMap.Singleton.ActionAddEvent(action.Item1, action.Item2);
        }
    }

    public static void RemoveDefaultActions()
    {
        foreach (Tuple<string, InputEvent> action in _defaultActions)
        {
            if (!InputMap.Singleton.HasAction(action.Item1))
            {
                continue;
            }

            InputMap.Singleton.EraseAction(action.Item1);
        }
    }
}
