// meta-description: Base template for extensible YAT commands.

using _BINDINGS_NAMESPACE_;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("_CLASS_", "Lorem ipsum dolor sit amet.", "[b]Usage[/b]: _CLASS_")]
	public partial class _CLASS_ : Extensible, ICommand
	{
		public YAT Yat { get; set; }

		public _CLASS_(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(YAT yat, params string[] args)
		{
			var option = args[1];

			if (Extensions.ContainsKey(option))
			{
				var extension = Extensions[option];
				return extension.Execute(yat, this, args[1..]);
			}

			yat.Terminal.Println("Options not found.", Terminal.PrintType.Error);
			return CommandResult.Failure;
		}
	}
}
