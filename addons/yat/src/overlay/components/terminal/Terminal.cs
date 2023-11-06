using System.Linq;
using System.Text;
using Godot;
using YAT.Commands;
using YAT.Helpers;

namespace YAT
{
	public partial class Terminal : Control
	{
		/// <summary>
		/// Delegate for the CommandExecuted event.
		/// </summary>
		/// <param name="command">The command that was executed.</param>
		/// <param name="args">The arguments passed to the command.</param>
		/// <param name="result">The result of the command execution.</param>
		[Signal]
		public delegate void CommandExecutedEventHandler(string command, string[] args, CommandResult result);

		public LineEdit Input;
		public RichTextLabel Output;

		/// <summary>
		/// The type of message to print in the YatTerminal.
		/// </summary>
		public enum PrintType
		{
			/// <summary>
			/// Represents the normal state of the YatTerminal component.
			/// </summary>
			Normal,
			/// <summary>
			/// Displays a error message in the terminal.
			/// </summary>
			Error,
			/// <summary>
			/// Displays a warning message in the terminal.
			/// </summary>
			Warning,
			/// <summary>
			/// Displays a success message in the terminal.
			/// </summary>
			Success
		}

		private YAT _yat;
		private Label _promptLabel;
		private string _prompt = "> ";
		private PanelContainer _window;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");
			_yat.OptionsChanged += UpdateOptions;

			_window = GetNode<PanelContainer>("Window/PanelContainer");
			_promptLabel = GetNode<Label>("%PromptLabel");

			Output = GetNode<RichTextLabel>("%Output");
			Output.MetaClicked += (link) => OS.ShellOpen((string)link);

			Input = GetNode<LineEdit>("%Input");
			Input.TextSubmitted += OnCommandSubmitted;
		}

		public override void _Input(InputEvent @event)
		{
			// Handle history navigation if the Terminal window is open.
			if (IsInsideTree())
			{
				if (@event.IsActionPressed("yat_history_previous"))
				{
					if (_yat.HistoryNode == null && _yat.History.Count > 0)
					{
						_yat.HistoryNode = _yat.History.Last;
						Input.Text = _yat.HistoryNode.Value;
					}
					else if (_yat.HistoryNode?.Previous != null)
					{
						_yat.HistoryNode = _yat.HistoryNode.Previous;
						Input.Text = _yat.HistoryNode.Value;
					}
				}

				if (@event.IsActionPressed("yat_history_next"))
				{
					if (_yat.HistoryNode != null && _yat.HistoryNode.Next != null)
					{
						_yat.HistoryNode = _yat.HistoryNode.Next;
						Input.Text = _yat.HistoryNode.Value;
					}
					else
					{
						_yat.HistoryNode = null;
						Input.Text = string.Empty;
					}
				}
			}
		}

		private void UpdateOptions(YatOptions options)
		{
			_promptLabel.Text = options.Prompt;
			_promptLabel.Visible = options.ShowPrompt;
			_window.Size = new(options.DefaultWidth, options.DefaultHeight);
			Output.ScrollFollowing = options.AutoScroll;
		}

		/// <summary>
		/// Prints the specified text to the terminal window, followed by a newline character.
		/// </summary>
		/// <param name="text">The text to print.</param>
		/// <param name="type">The type of print to use (e.g. error, warning, success, normal).</param>
		public void Println(string text, PrintType type = PrintType.Normal) => Print(text + '\n', type);

		/// <summary>
		/// Prints the specified text to the terminal with the specified print type.
		/// </summary>
		/// <param name="text">The text to print.</param>
		/// <param name="type">The type of print to use (e.g. error, warning, success, normal).</param>
		public void Print(string text, PrintType type = PrintType.Normal)
		{
			StringBuilder sb = new();
			var color = type switch
			{
				PrintType.Error => _yat.Options.ErrorColor,
				PrintType.Warning => _yat.Options.WarningColor,
				PrintType.Success => _yat.Options.SuccessColor,
				PrintType.Normal => _yat.Options.OutputColor,
				_ => _yat.Options.OutputColor,
			};

			sb.Append("[color=");
			sb.Append(color.ToHtml());
			sb.Append(']');
			sb.Append(text);
			sb.Append("[/color]");

			Output.AppendText(sb.ToString());
		}

		/// <summary>
		/// Clears the output text of the terminal window.
		/// </summary>
		public void Clear() => Output.Clear();

		/// <summary>
		/// Executes the given CLI command.
		/// </summary>
		/// <param name="input">The input arguments for the command.</param>
		private void ExecuteCommand(string[] input)
		{
			string commandName = input[0];
			if (!_yat.Commands.ContainsKey(commandName))
			{
				LogHelper.UnknownCommand(commandName);
				return;
			}

			ICommand command = _yat.Commands[commandName];
			var result = command.Execute(_yat, input);

			EmitSignal(SignalName.CommandExecuted, commandName, input, (ushort)result);
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
			if (_yat.History.Count > _yat.Options.HistoryLimit) _yat.History.RemoveFirst();

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
}
