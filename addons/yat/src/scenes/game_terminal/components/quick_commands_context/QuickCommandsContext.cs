using System.Collections.Generic;
using YAT.Helpers;

namespace YAT.Scenes;

public partial class QuickCommandsContext : ContextSubmenu
{
#nullable disable
    private YAT _yat;
    private TerminalSwitcher _terminalSwitcher;
#nullable restore

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
        _ = _yat.Commands.GetQuickCommands();
        Clear();

        foreach (KeyValuePair<string, string> qc in _yat.Commands.QuickCommands.Commands)
        {
            AddItem(qc.Key);
        }
    }

    private void OnQuickCommandsPressed(long id)
    {
        string key = GetItemText((int)id);

        if (!_yat.Commands.QuickCommands.Commands.TryGetValue(key, out var command))
        {
            return;
        }

        _ = _terminalSwitcher.CurrentTerminal.CommandManager.Run(
            Text.SanitizeText(command),
            _terminalSwitcher.CurrentTerminal
        );
    }
}
