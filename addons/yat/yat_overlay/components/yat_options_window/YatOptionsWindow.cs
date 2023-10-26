using Godot;

public partial class YatOptionsWindow : Control
{
	private YAT _yat;
	private YatWindow _window;
	private YatOptions _options;

	private Button _save;

	private LineEdit _prompt;
	private CheckBox _movable;
	private SpinBox _width;
	private SpinBox _height;
	private CheckBox _autoScroll;
	private CheckBox _showPrompt;

	private const string _optionsPath = "user://yat_options.tres";

	public override void _Ready()
	{
		_yat = GetNode<YAT>("/root/YAT");
		_options = _yat.Options;

		_save = GetNode<Button>("%Save");
		_save.Pressed += OnSaveButtonPressed;

		_window = GetNode<YatWindow>("YatWindow");
		_window.CloseRequested += () => QueueFree();

		_prompt = GetNode<LineEdit>("%Prompt");
		_prompt.Text = _options.Prompt;

		_movable = GetNode<CheckBox>("%Movable");
		_movable.ButtonPressed = _options.WindowMovable;

		_width = GetNode<SpinBox>("%Width");
		_width.Value = _options.DefaultWidth;

		_height = GetNode<SpinBox>("%Height");
		_height.Value = _options.DefaultHeight;

		_autoScroll = GetNode<CheckBox>("%AutoScroll");
		_autoScroll.ButtonPressed = _options.AutoScroll;

		_showPrompt = GetNode<CheckBox>("%ShowPrompt");
		_showPrompt.ButtonPressed = _options.ShowPrompt;

		ConnectSignals();
	}

	/// <summary>
	/// Called when the "Save" button is pressed. Saves the current options to a file.
	/// </summary>
	private void OnSaveButtonPressed()
	{
		// The actual directory paths for user:// are:
		// Windows: %APPDATA%\Godot\app_userdata\[project_name]
		// Linux: ~/.local/share/godot/app_userdata/[project_name]
		// macOS: ~/Library/Application Support/Godot/app_userdata/[project_name]
		switch (ResourceSaver.Save(_options, _optionsPath))
		{
			case Error.Ok:
				_yat.Terminal.Println("Options saved successfully.");
				GD.Print("Options saved successfully.");
				break;
			default:
				_yat.Terminal.Println("Failed to save options.");
				GD.PrintErr("Failed to save options.");
				break;
		}
	}

	private void ConnectSignals()
	{
		_prompt.TextSubmitted += (string text) =>
		{
			_options.Prompt = text;
			UpdateOptions();
		};

		_movable.Toggled += (bool pressed) =>
		{
			_options.WindowMovable = pressed;
			UpdateOptions();
		};

		_width.ValueChanged += (double value) =>
		{
			_options.DefaultWidth = (ushort)value;
			UpdateOptions();
		};

		_height.ValueChanged += (double value) =>
		{
			_options.DefaultHeight = (ushort)value;
			UpdateOptions();
		};

		_autoScroll.Toggled += (bool pressed) =>
		{
			_options.AutoScroll = pressed;
			UpdateOptions();
		};

		_showPrompt.Toggled += (bool pressed) =>
		{
			_options.ShowPrompt = pressed;
			UpdateOptions();
		};
	}

	private void UpdateOptions()
	{
		_options.EmitSignal(nameof(_options.OptionsChanged), _options);
	}
}
