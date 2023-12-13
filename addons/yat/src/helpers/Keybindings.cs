using Godot;
using Godot.Collections;

namespace YAT.Helpers
{
	public static class Keybindings
	{
		private static readonly Array<Dictionary> _defaultActions = new()
		{
			new()
			{
				{"name", "yat_toggle"},
				{"type", (short)ActionType.Key},
				{"key", (long)Key.Quoteleft},
			},
			new()
			{
				{"name", "yat_context_menu"},
				{"type", (short)ActionType.Mouse},
				{"mask", (long)MouseButtonMask.Right},
				{"index", (short)MouseButton.Right}
			},
			new()
			{
				{"name", "yat_terminal_history_previous"},
				{"type", (short)ActionType.Key},
				{"key", (long)Key.Up},
			},
			new()
			{
				{"name", "yat_terminal_history_next"},
				{"type", (short)ActionType.Key},
				{"key", (long)Key.Down},
			},
			new()
			{
				{"name", "yat_terminal_interrupt"},
				{"type", (short)ActionType.Key},
				{"key", (long)Key.C},
				{"ctrl", true}
			},
			new()
			{
				{"name", "yat_terminal_autocompletion"},
				{"type", (short)ActionType.Key},
				{"key", (long)Key.Tab},
			},
			new()
			{
				{"name", "yat_example_player_move_left"},
				{"type", (short)ActionType.Key},
				{"key", (long)Key.A},
			},
			new()
			{
				{"name", "yat_example_player_move_right"},
				{"type", (short)ActionType.Key},
				{"key", (long)Key.D},
			},
			new()
			{
				{"name", "yat_example_player_move_forward"},
				{"type", (short)ActionType.Key},
				{"key", (long)Key.W},
			},
			new()
			{
				{"name", "yat_example_player_move_backward"},
				{"type", (short)ActionType.Key},
				{"key", (long)Key.S},
			}
		};

		private enum ActionType
		{
			Key,
			Mouse
		}

		public static void LoadDefaultActions()
		{
			foreach (var action in _defaultActions)
			{
				string name = action["name"].ToString();

				if (InputMap.HasAction(name)) continue;

				InputMap.AddAction((string)action["name"]);

				ActionType type = action["type"].As<ActionType>();
				InputEvent iEvent;

				if (type == ActionType.Key)
				{
					iEvent = new InputEventKey
					{
						PhysicalKeycode = action["key"].As<Key>(),
						CtrlPressed = action.TryGetValue("ctrl", out var ctrl) && (bool)ctrl
					};
				}
				else
				{
					iEvent = new InputEventMouseButton
					{
						ButtonMask = action["mask"].As<MouseButtonMask>(),
						ButtonIndex = action["index"].As<MouseButton>(),
						CtrlPressed = action.TryGetValue("ctrl", out var ctrl) && (bool)ctrl
					};
				}

				InputMap.ActionAddEvent(name, iEvent);
			}
		}
	}
}
