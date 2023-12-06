using Godot;
using YAT.Scenes.YatWindow;

public partial class SettingsWindow : YatWindow
{
	private YAT.YAT _yat;

	private Button _save;
	private Button _load;
	private Button _restoreDefaults;

	private LineEdit _prompt;
	private CheckBox _movable;
	private SpinBox _width;
	private SpinBox _height;
	private CheckBox _autoScroll;
	private CheckBox _showPrompt;

	public override void _Ready()
	{
		_yat = GetNode<YAT.YAT>("/root/YAT");

		_save = GetNode<Button>("%Save");
		_save.Pressed += _yat.OptionsManager.Save;

		_load = GetNode<Button>("%Load");
		_load.Pressed += _yat.OptionsManager.Load;

		_restoreDefaults = GetNode<Button>("%RestoreDefaults");
		_restoreDefaults.Pressed += _yat.OptionsManager.RestoreDefaults;

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
		CloseRequested += () => QueueFree();

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
