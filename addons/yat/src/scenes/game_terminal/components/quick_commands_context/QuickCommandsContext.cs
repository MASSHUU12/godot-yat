using YAT.Helpers;

namespace YAT.Scenes;

public partial class QuickCommandsContext : ContextSubmenu
{
	private YAT _yat;
	private TerminalSwitcher _terminalSwitcher;

	public override void _Ready()
	{
		base._Ready();

		_yat = GetNode<YAT>("/root/YAT");
		_yat.Commands.QuickCommandsChanged += GetQuickCommands;

		_terminalSwitcher = GetNode<TerminalSwitcher>("../../Content/TerminalSwitcher");

		IdPressed += OnQuickCommandsPressed;

		GetQuickCommands();
	}

	private void GetQuickCommands()
	{
		_yat.Commands.GetQuickCommands();
		Clear();

		foreach (var qc in _yat.Commands.QuickCommands.Commands) AddItem(qc.Key);
	}

	private void OnQuickCommandsPressed(long id)
	{
		var key = GetItemText((int)id);

		if (!_yat.Commands.QuickCommands.Commands.TryGetValue(key, out var command)) return;

		_terminalSwitcher.CurrentTerminal.CommandManager.Run(
			Text.SanitizeText(command), _terminalSwitcher.CurrentTerminal
		);
	}
}
