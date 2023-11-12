using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Overlay.Components.Terminal;

namespace YAT.Commands
{
	[Command("set", "Sets a variable to a value.", "[b]Usage[/b]: set [i]variable[/i] [i]value[/i]")]
	public partial class Set : Extensible, ICommand
	{
		public YAT Yat { get; set; }

		public Set(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			if (args.Length < 3)
			{
				LogHelper.MissingArguments("set", "variable", "value");
				return CommandResult.InvalidArguments;
			}

			var variable = args[1];

			if (Extensions.ContainsKey(variable))
			{
				var extension = Extensions[variable];
				return extension.Execute(this, args[1..]);
			}

			Yat.Terminal.Print("Variable not found.", Terminal.PrintType.Error);
			return CommandResult.Failure;
		}
	}
}
