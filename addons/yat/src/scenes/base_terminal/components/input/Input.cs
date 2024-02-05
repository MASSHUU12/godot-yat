using Godot;
using YAT.Classes;

namespace YAT.Scenes.BaseTerminal;

public partial class Input : LineEdit
{
	private YAT _yat;
	private BaseTerminal _terminal;

	public override void _Ready()
	{
		_yat = GetNode<YAT>("/root/YAT");
		_yat.YatReady += () =>
		{
			// This 'fixes' the issue where terminal toggle key is writing to the input
			_yat.TerminalManager.TerminalOpened += () => { GrabFocus(); Clear(); };
		};

		_terminal = GetNode<BaseTerminal>("../../../../../");

		TextSubmitted += OnTextSubmitted;
	}

	private void OnTextSubmitted(string input)
	{
		if (_terminal.Locked || string.IsNullOrEmpty(input)) return;

		// If the input string starts with a specified character,
		// treat it as a method call on the selected node
		if (input.StartsWith('$'))
		{
			_terminal.SelectedNode.CallMethods(input[1..]);
			AddToTheHistory(input);
			Clear();
			return;
		}

		var command = Parser.ParseCommand(input);

		if (command.Length == 0) return;

		AddToTheHistory(input);

		_terminal.CommandManager.Run(command, _terminal);
		Clear();
	}

	private void AddToTheHistory(string command)
	{
		_terminal.HistoryNode = null;
		_terminal.History.AddLast(command);
		if (_terminal.History.Count > _yat.OptionsManager.Options.HistoryLimit) _terminal.History.RemoveFirst();
	}

	/// <summary>
	/// Moves the caret to the end of the input text.
	/// </summary>
	public void MoveCaretToEnd()
	{
		CaretColumn = Text.Length;
	}
}
