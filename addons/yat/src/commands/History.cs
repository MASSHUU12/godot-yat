using System.Linq;
using System.Text;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("history", aliases: "hist")]
[Description("Manages the command history of the current session.")]
[Argument("action", "clear|list|int(0:)", "The action to perform.")]
public sealed class History : ICommand
{
#nullable disable
    private BaseTerminal _terminal;
    private HistoryComponent _historyComponent;
#nullable restore

    public CommandResult Execute(CommandData data)
    {
        _terminal = data.Terminal;
        _historyComponent = _terminal.HistoryComponent;

        return data.Arguments["action"] switch
        {
            "clear" => ClearHistory(),
            "list" => ShowHistory(),
            _ => int.TryParse(data.RawData[1], out int index)
                ? ExecuteFromHistory(index)
                : ICommand.Failure($"Invalid action: {data.RawData[1]}"),
        };
    }

    private CommandResult ClearHistory()
    {
        _historyComponent.History.Clear();
        return ICommand.Success("History cleared.");
    }

    private CommandResult ExecuteFromHistory(int index)
    {
        if (index < 0 || index >= _historyComponent.History.Count)
        {
            return ICommand.Failure($"Invalid index: {index}");
        }

        string command = _historyComponent.History.ElementAt(index);
        if (command.StartsWith("history", "hist"))
        {
            return ICommand.InvalidCommand(
                "Cannot execute history command from history."
            );
        }

        _terminal.Print(
            $"Executing command at index {index}: {Text.EscapeBBCode(command)}"
        );
        _ = _terminal.CommandManager.Run(Text.SanitizeText(command), _terminal);

        return ICommand.Success();
    }

    private CommandResult ShowHistory()
    {
        StringBuilder sb = new();
        _ = sb.AppendLine("Terminal history:");

        ushort i = 0;
        foreach (string command in _historyComponent.History)
        {
            _ = sb.AppendLine($"{i++}: {Text.EscapeBBCode(command)}");
        }

        return ICommand.Ok(sb.ToString());
    }
}
