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
			BaseTerminal.CloseRequested += () => _yat.ToggleTerminal();

			MoveToCenter();
		}
	}
}
