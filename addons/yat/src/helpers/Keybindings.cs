using System;
using System.Collections.Generic;
using Godot;

namespace YAT.Helpers
{
	public static class Keybindings
	{
		private static readonly List<Tuple<string, InputEvent>> _defaultActions = new()
		{
			new("yat_toggle", new InputEventKey { PhysicalKeycode = Key.Quoteleft }),
			new("yat_context_menu", new InputEventMouseButton {
				ButtonMask = MouseButtonMask.Right,
				ButtonIndex = MouseButton.Right
			}),
			new("yat_terminal_history_previous", new InputEventKey { PhysicalKeycode = Key.Up }),
			new("yat_terminal_history_next", new InputEventKey { PhysicalKeycode = Key.Down }),
			new("yat_terminal_interrupt", new InputEventKey {
				PhysicalKeycode = Key.C,
				CtrlPressed = true
			}),
			new("yat_terminal_autocompletion_next", new InputEventKey { PhysicalKeycode = Key.Tab }),
			new("yat_terminal_autocompletion_previous", new InputEventKey {
				PhysicalKeycode = Key.Tab,
				ShiftPressed = true
			}),
			new("yat_example_player_move_left", new InputEventKey { PhysicalKeycode = Key.A }),
			new("yat_example_player_move_right", new InputEventKey { PhysicalKeycode = Key.D }),
			new("yat_example_player_move_forward", new InputEventKey { PhysicalKeycode = Key.W }),
			new("yat_example_player_move_backward", new InputEventKey { PhysicalKeycode = Key.S }),
		};

		public static void LoadDefaultActions()
		{
			foreach (var action in _defaultActions)
			{
				if (InputMap.HasAction(action.Item1)) continue;

				InputMap.AddAction(action.Item1);
				InputMap.ActionAddEvent(action.Item1, action.Item2);
			}
		}
	}
}
