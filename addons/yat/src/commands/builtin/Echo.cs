using YAT.Attributes;
using YAT.Helpers;

namespace YAT.Commands
{
	[Command("echo", "Displays the given text.", "[b]Usage[/b]: echo [i]text[/i]")]
	public partial class Echo : ICommand
	{
		public YAT Yat { get; set; }

		public Echo(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			if (args.Length < 2)
			{
				LogHelper.MissingArguments("echo", "text");
				return CommandResult.InvalidArguments;
			}

			var text = string.Join(" ", args[1..^0]);
			Yat.Terminal.Print(text);

			return CommandResult.Success;
		}
	}
}
