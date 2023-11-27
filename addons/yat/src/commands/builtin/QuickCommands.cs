using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("quickcommands", "Manages quick commands.", "[b]Usage[/b]: quickcommands [i]action[/i] [i]name[/i] [i]command[/i]", "qc")]
	[Arguments("action:[add, remove, list]", "name:string", "command:string")]
	public sealed class QuickCommands : ICommand
	{
		public YAT Yat { get; set; }

		public QuickCommands(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(System.Collections.Generic.Dictionary<string, object> cArgs, params string[] args)
		{
			return CommandResult.Success;
		}
	}
}
