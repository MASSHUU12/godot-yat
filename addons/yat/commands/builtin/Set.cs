using Godot;

namespace YAT.Commands
{
	[Command("set", "Sets a variable to a value.", "[b]Usage[/b]: set [i]variable[/i] [i]value[/i]")]
	public partial class Set : ICommand
	{
		public Extensible Extension = new();

		public CommandResult Execute(YAT yat, params string[] args)
		{
			if (args.Length < 3)
			{
				yat.Terminal.Println("Invalid arguments.", Terminal.PrintType.Error);
				return CommandResult.InvalidArguments;
			}

			var variable = args[1];

			if (Extension.Extensions.ContainsKey(variable))
			{
				var extension = Extension.Extensions[variable];
				return extension.Execute(yat, this, args[2..]);
			}

			yat.Terminal.Println("Variable not found.", Terminal.PrintType.Error);
			return CommandResult.Failure;
		}
	}
}
