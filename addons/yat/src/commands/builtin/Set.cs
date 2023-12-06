using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes.Terminal;

namespace YAT.Commands
{
	[Command("set", "Sets a variable to a value.", "[b]Usage[/b]: set [i]variable[/i] [i]value[/i]")]
	[Argument("variable", "string", "The name of the variable to set.")]
	[Argument("value", "string", "The value to set the variable to.")]
	public partial class Set : Extensible, ICommand
	{
		public YAT Yat { get; set; }

		public Set(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
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
