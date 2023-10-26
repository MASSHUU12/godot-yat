using Godot;

namespace YAT
{
	public partial class OptionsWindow : Control
	{
		private YAT _yat;
		private Window _window;

		private Button _save;
		private Button _load;

		private LineEdit _prompt;
		private CheckBox _movable;
		private SpinBox _width;
		private SpinBox _height;
		private CheckBox _autoScroll;
		private CheckBox _showPrompt;

		/// <summary>
		/// <list type="unordered">
		/// <item> The actual directory paths for user:// are: </item>
		/// <item> Windows: %APPDATA%\Godot\app_userdata\[project_name] </item>
		/// <item> Linux: ~/.local/share/godot/app_userdata/[project_name] </item>
		/// <item> macOS: ~/Library/Application Support/Godot/app_userdata/[project_name] </item>
		/// </list>
		/// </summary>
		private const string _optionsPath = "user://yat_options.tres";

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");

			_save = GetNode<Button>("%Save");
			_save.Pressed += _yat.OptionsManager.Save;

			_load = GetNode<Button>("%Load");
			_load.Pressed += _yat.OptionsManager.Load;

			_window = GetNode<Window>("Window");
			_window.CloseRequested += () => QueueFree();

			_prompt = GetNode<LineEdit>("%Prompt");
			_prompt.Text = _yat.Options.Prompt;

			_movable = GetNode<CheckBox>("%Movable");
			_movable.ButtonPressed = _yat.Options.WindowMovable;

			_width = GetNode<SpinBox>("%Width");
			_width.Value = _yat.Options.DefaultWidth;

			_height = GetNode<SpinBox>("%Height");
			_height.Value = _yat.Options.DefaultHeight;

			_autoScroll = GetNode<CheckBox>("%AutoScroll");
			_autoScroll.ButtonPressed = _yat.Options.AutoScroll;

			_showPrompt = GetNode<CheckBox>("%ShowPrompt");
			_showPrompt.ButtonPressed = _yat.Options.ShowPrompt;

			ConnectSignals();
		}

		private void ConnectSignals()
		{
			_prompt.TextSubmitted += (string text) =>
			{
				_yat.Options.Prompt = text;
				UpdateOptions();
			};

			_movable.Toggled += (bool pressed) =>
			{
				_yat.Options.WindowMovable = pressed;
				UpdateOptions();
			};

			_width.ValueChanged += (double value) =>
			{
				_yat.Options.DefaultWidth = (ushort)value;
				UpdateOptions();
			};

			_height.ValueChanged += (double value) =>
			{
				_yat.Options.DefaultHeight = (ushort)value;
				UpdateOptions();
			};

			_autoScroll.Toggled += (bool pressed) =>
			{
				_yat.Options.AutoScroll = pressed;
				UpdateOptions();
			};

			_showPrompt.Toggled += (bool pressed) =>
			{
				_yat.Options.ShowPrompt = pressed;
				UpdateOptions();
			};
		}

		private void UpdateOptions() => _yat.EmitSignal(nameof(_yat.OptionsChanged), _yat.Options);
	}
}
