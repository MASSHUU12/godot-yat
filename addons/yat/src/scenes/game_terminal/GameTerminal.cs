using Godot;

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

			BaseTerminal = GetNode<BaseTerminal.BaseTerminal>("Content/BaseTerminal");
			BaseTerminal.TitleChanged += title => Title = title;
			CloseRequested += _yat.CloseTerminal;

			MoveToCenter();
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event.IsActionPressed("yat_toggle"))
			{
				CallDeferred("emit_signal", SignalName.CloseRequested);
			}
		}
	}
}
