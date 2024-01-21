using Godot;
using YAT.Helpers;

namespace YAT.Scenes.GameTerminal
{
	public partial class GameTerminal : YatWindow.YatWindow
	{
		public BaseTerminal.BaseTerminal BaseTerminal { get; private set; }

		private YAT _yat;

		public override void _Ready()
		{
			base._Ready();

			_yat = GetNode<YAT>("/root/YAT");
			_yat.YatReady += () =>
			{
				CloseRequested += _yat.TerminalManager.CloseTerminal;
			};

			BaseTerminal = GetNode<BaseTerminal.BaseTerminal>("Content/BaseTerminal");
			BaseTerminal.TitleChangeRequested += title => Title = title;
			BaseTerminal.PositionResetRequested += ResetPosition;
			BaseTerminal.SizeResetRequested += () => Size = InitialSize;

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
