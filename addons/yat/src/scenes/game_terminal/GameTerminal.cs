using Godot;
using YAT.Helpers;
using YAT.Scenes.GameTerminal.Components;

namespace YAT.Scenes.GameTerminal
{
	public partial class GameTerminal : YatWindow.YatWindow
	{
		public BaseTerminal.BaseTerminal CurrentTerminal { get; private set; }
		public TerminalSwitcher TerminalSwitcher { get; private set; }

		private YAT _yat;

		public override void _Ready()
		{
			base._Ready();

			_yat = GetNode<YAT>("/root/YAT");
			_yat.YatReady += () =>
			{
				CloseRequested += _yat.TerminalManager.CloseTerminal;
			};

			TerminalSwitcher = GetNode<TerminalSwitcher>("%TerminalSwitcher");

			CurrentTerminal = TerminalSwitcher.CurrentTerminal;
			CurrentTerminal.TitleChangeRequested += title => Title = title;
			CurrentTerminal.PositionResetRequested += ResetPosition;
			CurrentTerminal.SizeResetRequested += () => Size = InitialSize;

			ContextMenu.AddSubmenuItem("QuickCommands", "QuickCommandsContext");

			MoveToCenter();
		}

		public override void _Input(InputEvent @event)
		{
			if (@event.IsActionPressed(Keybindings.TerminalToggle))
			{
				CallDeferred("emit_signal", SignalName.CloseRequested);
			}
		}
	}
}
