namespace YAT.Scenes.Terminal
{
	public partial class TerminalContext : ContextMenu.ContextMenu
	{
		public QuickCommandsContext QuickCommands { get; private set; }

		public override void _Ready()
		{
			base._Ready();

			QuickCommands = GetNode<QuickCommandsContext>("QuickCommandsContext");

			AddSubmenuItem("QuickCommands", "QuickCommandsContext");
		}
	}
}
