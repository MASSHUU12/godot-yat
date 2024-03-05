using Godot;
using YAT.Classes;

namespace YAT.Scenes;

public partial class Input : LineEdit
{
	[Export] public BaseTerminal Terminal { get; set; }

	private YAT _yat;

	public override void _Ready()
	{
		_yat = GetNode<YAT>("/root/YAT");
		_yat.YatReady += () =>
		{
			// This 'fixes' the issue where terminal toggle key is writing to the input
			_yat.TerminalManager.TerminalOpened += () =>
			{
				Clear();

				if (!Terminal.Current || Terminal.FullWindowDisplay.IsOpen) return;

				GrabFocus();
			};
		};

		TextSubmitted += OnTextSubmitted;
	}

	private void OnTextSubmitted(string input)
	{
		if (Terminal.Locked || string.IsNullOrEmpty(input)) return;

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

	public void MoveCaretToEnd()
	{
		CaretColumn = Text.Length;
	}
}
