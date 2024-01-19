using Godot;

namespace YAT.Scenes.BaseTerminal
{
	public partial class Input : LineEdit
	{
		private YAT _yat;
		private BaseTerminal _terminal;
		private CommandManager.CommandManager _commandManager;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");
			// This 'fixes' the issue where terminal toggle key is writing to the input
			_yat.TerminalOpened += () => { GrabFocus(); Clear(); };

			_terminal = GetNode<BaseTerminal>("../../../../../");
			_commandManager = _yat.GetNode<CommandManager.CommandManager>("CommandManager");

			TextSubmitted += OnTextSubmitted;
		}

		/// <summary>
		/// Handles the submission of a command by sanitizing the input,
		/// executing the command, and clearing the input buffer.
		/// </summary>
		/// <param name="command">The command to be submitted.</param>
		private void OnTextSubmitted(string command)
		{
			// If the input string starts with a specified character,
			// treat it as a method call on the selected node
			if (command.StartsWith('$'))
			{
				_yat.Terminal.SelectedNode.ParseAndCallMethods(command[1..]);
				AddToTheHistory(command);
				Clear();
				return;
			}

			var input = Helpers.Text.SanitizeText(command);
			input = Helpers.Text.ConcatenateSentence(input);

			if (input.Length == 0 || _commandManager.Locked) return;

			AddToTheHistory(command);

			_commandManager.Run(input, _terminal);
			Clear();
		}

		private void AddToTheHistory(string command)
		{
			_terminal.HistoryNode = null;
			_terminal.History.AddLast(command);
			if (_terminal.History.Count > _yat.Options.HistoryLimit) _terminal.History.RemoveFirst();
		}

		/// <summary>
		/// Moves the caret to the end of the input text.
		/// </summary>
		public void MoveCaretToEnd()
		{
			CaretColumn = Text.Length;
		}
	}
}
