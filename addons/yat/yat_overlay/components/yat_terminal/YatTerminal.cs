using Godot;

public partial class YatTerminal : Control
{
	public LineEdit Input;
	public RichTextLabel Output;

	private PanelContainer _window;
	private Label _promptLabel;
	private YatOptions _options;
	private string _prompt = "> ";

	public override void _Ready()
	{
		_options = GetNode<YAT>("/root/YAT").Options;
		_options.OptionsChanged += UpdateOptions;

		_window = GetNode<PanelContainer>("YatWindow/PanelContainer");
		_promptLabel = GetNode<Label>("%PromptLabel");

		Output = GetNode<RichTextLabel>("%Output");
		Input = GetNode<LineEdit>("%Input");
	}

	private void UpdateOptions(YatOptions options)
	{
		_promptLabel.Text = options.Prompt;
		_promptLabel.Visible = options.ShowPrompt;
		_window.Size = new(options.DefaultWidth, options.DefaultHeight);
	}

	/// <summary>
	/// Prints the specified text to the terminal window, followed by a newline character.
	/// </summary>
	/// <param name="text">The text to print.</param>
	public void Println(string text)
	{
		Print(text + "\n");
	}

	/// <summary>
	/// Prints the specified text to the terminal window.
	/// </summary>
	/// <param name="text">The text to print.</param>
	public void Print(string text)
	{
		Output.AppendText(text);
		if (_options.AutoScroll) Output.ScrollToLine(Output.GetLineCount());
	}

	/// <summary>
	/// Clears the output text of the terminal window.
	/// </summary>
	public void Clear()
	{
		Output.Text = string.Empty;
	}
}
