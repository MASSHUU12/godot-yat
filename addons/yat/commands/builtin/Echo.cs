using static YAT.Terminal;

namespace YAT.Commands
{
	[Command("echo", "Displays the given text.", "[b]Usage[/b]: echo [i]text[/i]")]
	public partial class Echo : ICommand
	{
		public CommandResult Execute(YAT yat, params string[] args)
		{
			if (args.Length < 2)
			{
				yat.Terminal.Println("Invalid input.", PrintType.Error);
				return CommandResult.InvalidArguments;
			}

			var text = string.Join(" ", args[1..^0]);
			yat.Terminal.Println(text);

			return CommandResult.Success;
		}
	}

}
