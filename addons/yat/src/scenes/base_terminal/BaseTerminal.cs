using System.Collections.Generic;
using System.Text;
using Godot;
using YAT.Helpers;

namespace YAT.Scenes.BaseTerminal
{
	public partial class BaseTerminal : Control
	{
		[Signal] public delegate void CloseRequestedEventHandler();
		[Signal] public delegate void TitleChangeRequestedEventHandler(string title);

		public Input Input { get; private set; }
		public TerminalContext Context { get; private set; }
		public SelectedNode SelectedNode { get; private set; }

		public readonly LinkedList<string> History = new();
		public LinkedListNode<string> HistoryNode = null;

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
		private Label _selectedNodeLabel;
		private RichTextLabel Output;
		private string _prompt = "> ";
		private CommandManager.CommandManager _commandManager;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");
			_yat.OptionsChanged += UpdateOptions;

			SelectedNode = GetNode<SelectedNode>("SelectedNode");
			SelectedNode.CurrentNodeChanged += OnCurrentNodeChanged;

			Context = GetNode<TerminalContext>("TerminalContext");

			_commandManager = _yat.CommandManager;
			_commandManager.CommandStarted += (command, args) =>
				EmitSignal(SignalName.TitleChangeRequested, "YAT - " + command);
			_commandManager.CommandFinished += (command, args, result) =>
				EmitSignal(SignalName.TitleChangeRequested, "YAT");

			_promptLabel = GetNode<Label>("%PromptLabel");
			_selectedNodeLabel = GetNode<Label>("%SelectedNodePath");
			Input = GetNode<Input>("%Input");

			Output = GetNode<RichTextLabel>("%Output");
			Output.MetaClicked += (link) => OS.ShellOpen((string)link);

			OnCurrentNodeChanged(SelectedNode.Current);
			UpdateOptions(_yat.Options);
		}

		public override void _Input(InputEvent @event)
		{
			// Handle history navigation if the Terminal window is open.
			if (IsInsideTree())
			{
				if (@event.IsActionPressed(Keybindings.TerminalHistoryPrevious))
				{
					if (HistoryNode is null && History.Count > 0)
					{
						HistoryNode = History.Last;
						Input.Text = HistoryNode.Value;
					}
					else if (HistoryNode?.Previous is not null)
					{
						HistoryNode = HistoryNode.Previous;
						Input.Text = HistoryNode.Value;
					}

					Input.CallDeferred(nameof(Input.MoveCaretToEnd));
				}

				if (@event.IsActionPressed(Keybindings.TerminalHistoryNext))
				{
					if (HistoryNode is not null && HistoryNode.Next is not null)
					{
						HistoryNode = HistoryNode.Next;
						Input.Text = HistoryNode.Value;
					}
					else
					{
						HistoryNode = null;
						Input.Text = string.Empty;
					}

					Input.CallDeferred(nameof(Input.MoveCaretToEnd));
				}

				if (@event.IsActionPressed(Keybindings.TerminalInterrupt) &&
					_commandManager.Cts is not null)
				{
					Print("Command cancellation requested.", PrintType.Warning);

					_commandManager.Cts.Cancel();
					_commandManager.Cts.Dispose();
					_commandManager.Cts = null;
				}

				if (@event.IsActionPressed(Keybindings.ContextMenu))
				{
					Context.ShowNextToMouse();
				}
			}
		}

		private void UpdateOptions(YatOptions options)
		{
			OnCurrentNodeChanged(SelectedNode.Current);
			_promptLabel.Visible = options.ShowPrompt;
			Size = new((int)options.DefaultWidth, (int)options.DefaultHeight);
			Output.ScrollFollowing = options.AutoScroll;
		}

		private void OnCurrentNodeChanged(Node node)
		{
			_selectedNodeLabel.Text = Text.ShortenPath(node?.GetPath() ?? string.Empty, 20);
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

		public void Print(StringBuilder text, PrintType type = PrintType.Normal) => Print(text.ToString(), type);

		/// <summary>
		/// Clears the output text of the terminal window.
		/// </summary>
		public void Clear() => Output.Clear();
	}
}
