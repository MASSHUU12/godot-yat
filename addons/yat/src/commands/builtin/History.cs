using System.Linq;
using System.Text;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;

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
		public YAT Yat { get; set; }

		public History(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			switch (args[1])
			{
				case "clear":
					ClearHistory();
					break;
				case "list":
					ShowHistory();
					break;
				default:
					if (int.TryParse(args[1], out int index))
						ExecuteFromHistory(index);
					else
					{
						Yat.Terminal.Print($"Invalid action: {args[1]}");
						return CommandResult.Failure;
					}
					break;
			}

			return CommandResult.Success;
		}

		private void ClearHistory()
		{
			Yat.History.Clear();
			Yat.Terminal.Print("Terminal history cleared.");
		}

		private void ExecuteFromHistory(int index)
		{
			if (index < 0 || index >= Yat.History.Count)
			{
				Yat.Terminal.Print($"Invalid index: {index}");
				return;
			}

			var command = Yat.History.ElementAt(index);

			Yat.Terminal.Print(
				$"Executing command at index {index}: {Text.EscapeBBCode(command)}"
			);
			Yat.CommandManager.Run(Text.SanitizeText(command));
		}

		private void ShowHistory()
		{
			StringBuilder sb = new();

			sb.AppendLine("Terminal history:");

			int i = 0;
			foreach (string command in Yat.History)
			{
				sb.AppendLine($"{i++}: {Text.EscapeBBCode(command)}");
			}

			Yat.Terminal.Print(sb.ToString());
		}
	}
}
