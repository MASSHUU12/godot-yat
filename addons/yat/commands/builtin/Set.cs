namespace YAT.Commands
{
	[Command("set", "Sets a variable to a value.", "[b]Usage[/b]: set [i]variable[/i] [i]value[/i]")]
	public partial class Set : Extensible, ICommand
	{
		public CommandResult Execute(YAT yat, params string[] args)
		{
			if (args.Length < 3)
			{
				yat.Terminal.Println("Invalid arguments.", Terminal.PrintType.Error);
				return CommandResult.InvalidArguments;
			}

			var variable = args[1];

			if (Extensions.ContainsKey(variable))
			{
				var extension = Extensions[variable];
				return extension.Execute(yat, this, args[1..]);
			}

			yat.Terminal.Println("Variable not found.", Terminal.PrintType.Error);
			return CommandResult.Failure;
		}
	}
}
