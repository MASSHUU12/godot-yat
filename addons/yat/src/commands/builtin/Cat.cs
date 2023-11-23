using System.Collections.Generic;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("cat", "Prints content of a file.", "[b]Usage[/b]: cat [i]file[/i]")]
	[Arguments("file:string")]
	[Options("-l=int(1:99)")]
	public sealed class Cat : ICommand
	{
		public YAT Yat { get; set; }

		public Cat(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
		{
			int limit = -1;
			if (cArgs.TryGetValue("-l", out object l) && l is not null)
				limit = (int)l;

			Yat.Terminal.Print($"{limit}");

			return CommandResult.Success;
		}
	}
}
