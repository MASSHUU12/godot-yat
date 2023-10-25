using Godot;

public partial class YatOptionsWindow : Control
{
	private YatWindow _window;
	private YatOptions _options;

	private LineEdit _prompt;
	private CheckBox _movable;
	private SpinBox _width;
	private SpinBox _height;
	private CheckBox _autoScroll;
	private CheckBox _showPrompt;

	public override void _Ready()
	{
		_options = GetNode<YAT>("/root/YAT").Options;
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
