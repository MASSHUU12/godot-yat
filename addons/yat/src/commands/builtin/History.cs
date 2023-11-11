using System.Linq;
using System.Text;
using YAT.Helpers;

namespace YAT.Commands
{
	[Command(
		"history",
		"Displays the history of commands executed in the current session.",
		"[b]Usage[/b]: history [i]action (optional)[/i]" +
		"\n\n[b]Actions[/b]:\n" +
		"[b]clear[/b]: Clears the history.\n" +
		"[b]<number>[/b]: Executes the command at the specified index in the history.\n" +
		"[b]list[/b]: Lists the history.",
		"hist"
	)]
	public partial class History : ICommand
	{
		public YAT Yat { get; set; }

		public History(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			if (args.Length <= 1)
			{
				ShowHistory();
				return CommandResult.Success;
			}

			if (args.Length > 2)
			{
				Yat.Terminal.Print("Too many arguments.");
				return CommandResult.Failure;
			}

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
				$"Executing command at index {index}: {TextHelper.EscapeBBCode(command)}"
			);
			Yat.Terminal.CommandManager(TextHelper.SanitizeText(command));
		}

		private void ShowHistory()
		{
			StringBuilder sb = new();

			sb.AppendLine("Terminal history:");

			int i = 0;
			foreach (string command in Yat.History)
			{
				sb.AppendLine($"{i++}: {TextHelper.EscapeBBCode(command)}");
			}

			Yat.Terminal.Print(sb.ToString());
		}
	}
}
