using Godot;
using YAT.Classes;
using YAT.Classes.Managers;

namespace YAT.Scenes.BaseTerminal;

public partial class Input : LineEdit
{
	[Export] public BaseTerminal Terminal { get; set; }
	[Export] public MethodManager MethodManager { get; set; }

	private YAT _yat;

	public override void _Ready()
	{
		_yat = GetNode<YAT>("/root/YAT");
		_yat.YatReady += () =>
		{
			// This 'fixes' the issue where terminal toggle key is writing to the input
			_yat.TerminalManager.TerminalOpened += () => { GrabFocus(); Clear(); };
		};

		TextSubmitted += OnTextSubmitted;
	}

	private void OnTextSubmitted(string input)
	{
		if (Terminal.Locked || string.IsNullOrEmpty(input)) return;

		// If the input string starts with a specified character,
		// treat it as a method call on the selected node
		if (input.StartsWith('$'))
		{
			MethodManager.CallMethods(input[1..]);
			AddToTheHistory(input);
			Clear();
			return;
		}

		var command = Parser.ParseCommand(input);

		if (command.Length == 0) return;

		AddToTheHistory(input);

		Terminal.CommandManager.Run(command, Terminal);
		Clear();
	}

	private void AddToTheHistory(string command)
	{
		Terminal.HistoryNode = null;
		Terminal.History.AddLast(command);
		if (Terminal.History.Count > _yat.PreferencesManager.Preferences.HistoryLimit) Terminal.History.RemoveFirst();
	}

	/// <summary>
	/// Moves the caret to the end of the input text.
	/// </summary>
	public void MoveCaretToEnd()
	{
		CaretColumn = Text.Length;
	}
}
