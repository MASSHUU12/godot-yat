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
		"[b]list[/b]: Lists the history."
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

			return CommandResult.Success;
		}

		private void ShowHistory()
		{
			Yat.Terminal.Print("Terminal history:");
			int i = 0;
			foreach (string command in Yat.History)
			{
				Yat.Terminal.Print($"{i++}: {TextHelper.EscapeBBCode(command)}");
			}
		}
	}
}
