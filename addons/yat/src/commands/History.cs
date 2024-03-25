using System.Linq;
using System.Text;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("history", aliases: "hist")]
[Usage("history [i]action[/i]")]
[Description("Manages the command history of the current session.")]
[Argument("action", "clear|list|int(0:)", "The action to perform.")]
public sealed class History : ICommand
{
#nullable disable
	private BaseTerminal _terminal;
#nullable restore

	public CommandResult Execute(CommandData data)
	{
		_terminal = data.Terminal;

		switch (data.Arguments["action"])
		{
			case "clear":
				return ClearHistory();
			case "list":
				return ShowHistory();
			default:
				if (int.TryParse(data.RawData[1], out int index))
					return ExecuteFromHistory(index);
				else return ICommand.Failure($"Invalid action: {data.RawData[1]}");
		}
	}

	private CommandResult ClearHistory()
	{
		_terminal.History.Clear();
		return ICommand.Success("History cleared.");
	}

	private CommandResult ExecuteFromHistory(int index)
	{
		if (index < 0 || index >= _terminal.History.Count)
			return ICommand.Failure($"Invalid index: {index}");

		var command = _terminal.History.ElementAt(index);
		if (command.StartsWith("history", "hist")) return ICommand.InvalidCommand(
			"Cannot execute history command from history."
		);

		_terminal.Print(
			$"Executing command at index {index}: {Text.EscapeBBCode(command)}"
		);
		_terminal.CommandManager.Run(Text.SanitizeText(command), _terminal);

		return ICommand.Success();
	}

	private CommandResult ShowHistory()
	{
		StringBuilder sb = new();
		sb.AppendLine("Terminal history:");

		ushort i = 0;
		foreach (string command in _terminal.History)
			sb.AppendLine($"{i++}: {Text.EscapeBBCode(command)}");

		return ICommand.Ok(sb.ToString());
	}
}
