using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("echo", "Displays the given text.", "[b]Usage[/b]: echo [i]text[/i]")]
	[Arguments("message:string")]
	public partial class Echo : ICommand
	{
		public YAT Yat { get; set; }

		public Echo(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			var text = string.Join(" ", args[1..^0]);
			Yat.Terminal.Print(text);

			return CommandResult.Success;
		}
	}
}
