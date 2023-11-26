using Godot;

namespace YAT.Scenes.Overlay.Components.Terminal
{
	public partial class Terminal : YatWindow.YatWindow
	{
		public Input Input { get; private set; }

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
		private RichTextLabel Output;
		private string _prompt = "> ";
		private CommandManager _commandManager;
		private ContextMenu.ContextMenu _terminalContext;

		public override void _Ready()
		{
			base._Ready();

			_yat = GetNode<YAT>("/root/YAT");
			_yat.OptionsChanged += UpdateOptions;

			_terminalContext = GetNode<ContextMenu.ContextMenu>("%TerminalContext");

			_commandManager = _yat.GetNode<CommandManager>("CommandManager");
			_commandManager.CommandStarted += (command, args) => Title = "YAT - " + command;
			_commandManager.CommandFinished += (command, args, result) => Title = "YAT";

			_promptLabel = GetNode<Label>("%PromptLabel");
			Input = GetNode<Input>("%Input");

			Output = GetNode<RichTextLabel>("%Output");
			Output.MetaClicked += (link) => OS.ShellOpen((string)link);

			CloseRequested += () => _yat.ToggleOverlay();

			UpdateOptions(_yat.Options);
		}

		public override void _Input(InputEvent @event)
		{
			// Handle history navigation if the Terminal window is open.
			if (IsInsideTree())
			{
				if (@event.IsActionPressed("yat_terminal_history_previous"))
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

					Input.CallDeferred(nameof(Input.MoveCaretToEnd));
				}

				if (@event.IsActionPressed("yat_terminal_history_next"))
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

					Input.CallDeferred(nameof(Input.MoveCaretToEnd));
				}

				if (@event.IsActionPressed("yat_terminal_interrupt") &&
					_commandManager.Cts != null)
				{
					Print("Command cancellation requested.", PrintType.Warning);

					_commandManager.Cts.Cancel();
					_commandManager.Cts.Dispose();
					_commandManager.Cts = null;
				}

				if (@event.IsActionPressed("yat_context_menu"))
				{
					_terminalContext.ShowNextToMouse();
				}
			}

			if (@event.IsActionPressed("yat_toggle"))
			{
				CallDeferred("emit_signal", SignalName.CloseRequested);
			}
		}

		private void UpdateOptions(YatOptions options)
		{
			_promptLabel.Text = options.Prompt;
			_promptLabel.Visible = options.ShowPrompt;
			Size = new((int)options.DefaultWidth, (int)options.DefaultHeight);
			Output.ScrollFollowing = options.AutoScroll;
		}

		/// <summary>
		/// Prints the specified text to the terminal with the specified print type.
		/// </summary>
		/// <param name="text">The text to print.</param>
		/// <param name="type">The type of print to use (e.g. error, warning, success, normal).</param>
		public void Print(string text, PrintType type = PrintType.Normal)
		{
			var color = type switch
			{
				PrintType.Error => _yat.Options.ErrorColor,
				PrintType.Warning => _yat.Options.WarningColor,
				PrintType.Success => _yat.Options.SuccessColor,
				PrintType.Normal => _yat.Options.OutputColor,
				_ => _yat.Options.OutputColor,
			};

			// Using CallDeferred to avoid issues when running this method in a separate thread.
			Output.CallDeferred("push_color", color);
			// Paragraph is needed because newline characters are breaking formatting.
			Output.CallDeferred("push_paragraph", (ushort)HorizontalAlignment.Left);
			Output.CallDeferred("append_text", text);
			Output.CallDeferred("pop_all");
		}

		/// <summary>
		/// Clears the output text of the terminal window.
		/// </summary>
		public void Clear() => Output.Clear();
	}
}
