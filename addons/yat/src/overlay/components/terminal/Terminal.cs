using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Overlay.Components.Terminal
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

		public Input Input { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether the terminal is locked.
		/// When the terminal is locked, no commands can be executed.
		///
		/// Terminal is locked automatically when a command is executing.
		/// No need to lock it manually.
		/// </summary>
		public bool Locked { get; private set; }

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
		private RichTextLabel Output;
		private PanelContainer _window;
		private CancellationTokenSource _cts;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");
			_yat.OptionsChanged += UpdateOptions;

			_window = GetNode<PanelContainer>("Window/PanelContainer");
			_promptLabel = GetNode<Label>("%PromptLabel");

			Output = GetNode<RichTextLabel>("%Output");
			Output.MetaClicked += (link) => OS.ShellOpen((string)link);

			Input = GetNode<Input>("%Input");

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

				if (@event.IsActionPressed("yat_terminal_interrupt") && _cts != null)
				{
					Print("Command cancellation requested.", PrintType.Warning);

					_cts.Cancel();
					_cts.Dispose();
					_cts = null;
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

		/// <summary>
		/// Executes the given CLI command.
		/// </summary>
		/// <param name="input">The input arguments for the command.</param>
		private void ExecuteCommand(string[] input, Dictionary<string, object> cArgs)
		{
			string commandName = input[0];
			var command = _yat.Commands[commandName];

			Locked = true;
			var result = command.Execute(input);
			result = result == CommandResult.NotImplemented ? command.Execute(cArgs, input) : result;
			Locked = false;

			EmitSignal(SignalName.CommandExecuted, commandName, input, (ushort)result);
		}

		/// <summary>
		/// Executes a command in a separate thread,
		/// allowing the terminal to remain responsive.
		/// </summary>
		/// <param name="input">The command and its arguments.</param>
		private async void ExecuteThreadedCommand(string[] input, Dictionary<string, object> cArgs)
		{
			_cts = new();

			Task task = new(() =>
			{
				string commandName = input[0];
				var command = _yat.Commands[commandName];

				Locked = true;
				var result = command.Execute(input);
				result = result == CommandResult.NotImplemented ? command.Execute(cArgs, input) : result;
				Locked = false;

				CallDeferredThreadGroup(
					"emit_signal", SignalName.CommandExecuted, commandName, input, (ushort)result
				);
			}, _cts.Token);

			task.Start();

			await ToSignal(this, SignalName.CommandExecuted);

			Print("Command execution finished.", PrintType.Success);
		}

		/// <summary>
		/// Executes the specified command with the given arguments.
		/// </summary>
		/// <param name="args">The arguments to pass to the command.</param>
		public void CommandManager(string[] args)
		{
			if (args.Length == 0) return;

			string commandName = args[0];

			if (!_yat.Commands.ContainsKey(commandName))
			{
				LogHelper.UnknownCommand(commandName);
				return;
			}

			ICommand command = _yat.Commands[commandName];
			Dictionary<string, object> convertedArgs = null;

			if (command.GetAttribute<NoValidateAttribute>() is null)
			{
				if (!CommandHelper.ValidateCommandArguments(
					command, args[1..], out convertedArgs
				)) return;
			}

			if (AttributeHelper.GetAttribute<ThreadedAttribute>(
				_yat.Commands[commandName]
			) is not null)
			{
				ExecuteThreadedCommand(args, convertedArgs);
				return;
			}

			ExecuteCommand(args, convertedArgs);
		}
	}
}
