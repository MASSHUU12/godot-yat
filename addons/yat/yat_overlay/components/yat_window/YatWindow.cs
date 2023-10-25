using Godot;

public partial class YatWindow : Control
{
	[Signal] public delegate void CloseRequestedEventHandler();

	[Export] public string Title { get; set; } = "Title";
	[Export] public bool CanClose { get; set; } = true;

	private YatOptions _options;

	private PanelContainer _window;
	private Button _closeButton;
	private Label _title;

	public override void _Ready()
	{
		_options = GetNode<YAT>("/root/YAT").Options;

		_window = GetNode<PanelContainer>("PanelContainer");
		_window.GuiInput += OnGuiInput;

		_title = GetNode<Label>("PanelContainer/VBoxContainer/MarginContainer/Header/Title");
		_title.Text = Title;

		_closeButton = GetNode<Button>("PanelContainer/VBoxContainer/MarginContainer/Header/Close");

		if (CanClose)
		{
			_closeButton.Pressed += () => EmitSignal(SignalName.CloseRequested);
			_closeButton.Visible = true;
		}
		else _closeButton.Visible = false;

		MoveToTheDefaultPosition();
	}

	private void OnGuiInput(InputEvent @event)
	{
		// Move the window when dragging it.
		if (@event is InputEventMouseMotion && _options.WindowMovable)
		{
			var e = @event as InputEventMouseMotion;

			if (e.ButtonMask == MouseButtonMask.Left) Position += e.Relative;
		}
	}

	/// <summary>
	/// Moves the terminal window to the default position at the center of the viewport.
	/// </summary>
	private void MoveToTheDefaultPosition()
	{
		var center = GetViewportRect().GetCenter();

		_window.Position = new(
			center.X - _window.Size.X / 2,
			center.Y - _window.Size.Y / 2
		);
	}
}
