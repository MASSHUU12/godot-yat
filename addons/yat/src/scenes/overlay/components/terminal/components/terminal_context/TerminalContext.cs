using YAT.Helpers;

namespace YAT.Scenes.Overlay.Components.Terminal
{
	public partial class TerminalContext : ContextMenu.ContextMenu
	{
		private YAT _yat;
		private ContextSubmenu _quickCommands;

		public override void _Ready()
		{
			base._Ready();

			_yat = GetNode<YAT>("/root/YAT");

			_quickCommands = GetNode<ContextSubmenu>("QuickCommandsSubmenu");
			_quickCommands.IdPressed += OnQuickCommandsPressed;

			AddSubmenuItem("QuickCommands", "QuickCommandsSubmenu");
		}

		/// <summary>
		/// Event handler for when a quick command is pressed.
		/// </summary>
		/// <param name="id">The ID of the pressed command.</param>
		private void OnQuickCommandsPressed(long id)
		{
			var command = _quickCommands.GetItemText((int)id);

			_yat.CommandManager.Run(TextHelper.SanitizeText(command));
		}
	}
}
