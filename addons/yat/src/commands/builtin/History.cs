using System.Linq;
using System.Text;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes.BaseTerminal;

namespace YAT.Commands
{
	[Command(
		"history",
		"Manages the command history of the current session.",
		"[b]Usage[/b]: history [i]action[/i]" +
		"\n\n[b]Actions[/b]:\n" +
		"[b]clear[/b]: Clears the history.\n" +
		"[b]<number>[/b]: Executes the command at the specified index in the history.\n" +
		"[b]list[/b]: Lists the history.",
		"hist"
	)]
	[Argument("action", "[clear, list, int]", "The action to perform.")]
	public partial class History : ICommand
	{
		private YAT _yat;
		private BaseTerminal _terminal;

		public CommandResult Execute(CommandArguments args)
		{
			_yat = args.Yat;
			_terminal = args.Terminal;

			switch (args.Arguments[1])
			{
				case "clear":
					ClearHistory();
					break;
				case "list":
					ShowHistory();
					break;
				default:
					if (int.TryParse(args.Arguments[1], out int index)) ExecuteFromHistory(index);
					else
					{
						_terminal.Print($"Invalid action: {args.Arguments[1]}");
						return CommandResult.Failure;
					}
					break;
			}

			return CommandResult.Success;
		}

		private void ClearHistory()
		{
			_terminal.History.Clear();
			_terminal.Print("Terminal history cleared.");
		}

		private void ExecuteFromHistory(int index)
		{
			if (index < 0 || index >= _terminal.History.Count)
			{
				_terminal.Print($"Invalid index: {index}");
				return;
			}

			var command = _terminal.History.ElementAt(index);

			_terminal.Print(
				$"Executing command at index {index}: {Text.EscapeBBCode(command)}"
			);
			_yat.CommandManager.Run(Text.SanitizeText(command));
		}

		private void ShowHistory()
		{
			StringBuilder sb = new();
			sb.AppendLine("Terminal history:");

			ushort i = 0;
			foreach (string command in _terminal.History)
			{
				sb.AppendLine($"{i++}: {Text.EscapeBBCode(command)}");
			}

			_terminal.Print(sb);
		}
	}
}
