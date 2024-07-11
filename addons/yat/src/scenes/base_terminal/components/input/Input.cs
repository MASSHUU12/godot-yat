using Godot;
using YAT.Classes;

namespace YAT.Scenes;

public partial class Input : LineEdit
{
#nullable disable
    [Export] public BaseTerminal Terminal { get; set; }

    private YAT _yat;
#nullable restore

    public override void _Ready()
    {
        _yat = GetNode<YAT>("/root/YAT");
        _yat.YatReady += () =>
        {
            _yat.TerminalManager.TerminalOpened += () =>
            {
                if (Terminal.Current && !Terminal.FullWindowDisplay.IsOpen)
                {
                    GrabFocus();
                }
            };
        };

        TextSubmitted += OnTextSubmitted;
    }

    private void OnTextSubmitted(string input)
    {
        if (Terminal.Locked || string.IsNullOrEmpty(input))
        {
            return;
        }

        input = GetQuickCommand(input);

        string[] command = Parser.ParseCommand(input);

        if (command.Length == 0)
        {
            return;
        }

        Terminal.HistoryComponent.Add(input);
        _ = Terminal.CommandManager.Run(command, Terminal);
        Clear();
    }

    private string GetQuickCommand(string input)
    {
        return _yat.Commands.QuickCommands.Commands.TryGetValue(input, out string? value)
            ? value
            : input;
    }

    public void MoveCaretToEnd()
    {
        CaretColumn = Text.Length;
    }
}
