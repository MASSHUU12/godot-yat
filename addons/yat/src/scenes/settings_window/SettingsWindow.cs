using Godot;

namespace YAT.Scenes.SettingsWindow;

public partial class SettingsWindow : YatWindow.YatWindow
{
	private OptionsManager.OptionsManager _manager;

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
		base._Ready();

		_manager = GetNode<YAT>("/root/YAT").OptionsManager;

		_save = GetNode<Button>("%Save");
		_save.Pressed += _manager.Save;

		_load = GetNode<Button>("%Load");
		_load.Pressed += _manager.Load;

		_restoreDefaults = GetNode<Button>("%RestoreDefaults");
		_restoreDefaults.Pressed += _manager.RestoreDefaults;

		_prompt = GetNode<LineEdit>("%Prompt");
		_prompt.Text = _manager.Options.Prompt;

		_movable = GetNode<CheckBox>("%Movable");
		_movable.ButtonPressed = _manager.Options.WindowMovable;

		_width = GetNode<SpinBox>("%Width");
		_width.Value = _manager.Options.DefaultWidth;

		_height = GetNode<SpinBox>("%Height");
		_height.Value = _manager.Options.DefaultHeight;

		_autoScroll = GetNode<CheckBox>("%AutoScroll");
		_autoScroll.ButtonPressed = _manager.Options.AutoScroll;

		_showPrompt = GetNode<CheckBox>("%ShowPrompt");
		_showPrompt.ButtonPressed = _manager.Options.ShowPrompt;

		ConnectSignals();
	}

	private void ConnectSignals()
	{
		CloseRequested += () => QueueFree();

		_prompt.TextSubmitted += (string text) =>
		{
			_manager.Options.Prompt = text;
			UpdateOptions();
		};

		_movable.Toggled += (bool pressed) =>
		{
			_manager.Options.WindowMovable = pressed;
			UpdateOptions();
		};

		_width.ValueChanged += (double value) =>
		{
			_manager.Options.DefaultWidth = (ushort)value;
			UpdateOptions();
		};

		_height.ValueChanged += (double value) =>
		{
			_manager.Options.DefaultHeight = (ushort)value;
			UpdateOptions();
		};

		_autoScroll.Toggled += (bool pressed) =>
		{
			_manager.Options.AutoScroll = pressed;
			UpdateOptions();
		};

		_showPrompt.Toggled += (bool pressed) =>
		{
			_manager.Options.ShowPrompt = pressed;
			UpdateOptions();
		};
	}

	private void UpdateOptions() => _manager.EmitSignal(nameof(_manager.OptionsChanged), _manager.Options);
}
