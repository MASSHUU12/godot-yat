using System.Linq;
using Godot;

public partial class YatTerminal : Control
{
	public LineEdit Input;
	public RichTextLabel Output;

	private YAT _yat;
	private Label _promptLabel;
	private YatOptions _options;
	private string _prompt = "> ";
	private PanelContainer _window;

	public override void _Ready()
	{
		_yat = GetNode<YAT>("/root/YAT");
		_options = _yat.Options;
		_options.OptionsChanged += UpdateOptions;

		_window = GetNode<PanelContainer>("YatWindow/PanelContainer");
		_promptLabel = GetNode<Label>("%PromptLabel");

		Output = GetNode<RichTextLabel>("%Output");
		Input = GetNode<LineEdit>("%Input");
		Input.TextSubmitted += OnCommandSubmitted;
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

	/// <summary>
	/// Executes the given CLI command.
	/// </summary>
	/// <param name="input">The input arguments for the command.</param>
	private void ExecuteCommand(string[] input)
	{
		if (input.Length == 0)
		{
			Println("Invalid input.");
			return;
		}

		string commandName = input[0];
		if (!_yat.Commands.ContainsKey(commandName))
		{
			Println($"Unknown command: {commandName}");
			return;
		}

		IYatCommand command = _yat.Commands[commandName];
		command.Execute(input, _yat);
	}

	/// <summary>
	/// Handles the submission of a command by sanitizing the input,
	/// executing the command, and clearing the input buffer.
	/// </summary>
	/// <param name="command">The command to be submitted.</param>
	private void OnCommandSubmitted(string command)
	{
		var input = SanitizeInput(command);

		if (input.Length == 0) return;

		_yat.HistoryNode = null;
		_yat.History.AddLast(command);
		if (_yat.History.Count > _options.HistoryLimit) _yat.History.RemoveFirst();

		ExecuteCommand(input);
		Input.Clear();
	}

	/// <summary>
	/// Sanitizes the input command by removing leading/trailing white space
	/// and extra spaces between words.
	/// </summary>
	/// <param name="command">The command to sanitize.</param>
	/// <returns>The sanitized command.</returns>
	private static string[] SanitizeInput(string command)
	{
		command = command.Trim();
		return command.Split(' ').Where(
			s => !string.IsNullOrWhiteSpace(s)
		).ToArray();
	}
}
