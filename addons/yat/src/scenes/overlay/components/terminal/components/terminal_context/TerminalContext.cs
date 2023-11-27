using Godot;
using YAT.Helpers;

namespace YAT.Scenes.Overlay.Components.Terminal
{
	public partial class TerminalContext : ContextMenu.ContextMenu
	{
		[Export]
		public QuickCommands QuickCommands { get; set; } = new()
		{
			Commands = {
				{ "Quit", "quit"},
				{ "Hello", "watch echo 0.5 Hello"}
			}
		};

		private YAT _yat;
		private ContextSubmenu _quickCommands;

		public override void _Ready()
		{
			base._Ready();

			_yat = GetNode<YAT>("/root/YAT");

			_quickCommands = GetNode<ContextSubmenu>("QuickCommandsSubmenu");
			_quickCommands.IdPressed += OnQuickCommandsPressed;

			GetQuickCommands();
			AddSubmenuItem("QuickCommands", "QuickCommandsSubmenu");
		}

		/// <summary>
		/// Retrieves the quick commands and adds them to the list of quick commands.
		/// </summary>
		private void GetQuickCommands()
		{
			_quickCommands.Clear();

			foreach (var command in QuickCommands.Commands)
			{
				_quickCommands.AddItem(command.Key);
			}
		}

		/// <summary>
		/// Event handler for when a quick command is pressed.
		/// </summary>
		/// <param name="id">The ID of the pressed command.</param>
		private void OnQuickCommandsPressed(long id)
		{
			var key = _quickCommands.GetItemText((int)id);

			if (!QuickCommands.Commands.TryGetValue(key, out var command))
			{
				return;
			}

			_yat.CommandManager.Run(TextHelper.SanitizeText(command));
		}
	}
}
