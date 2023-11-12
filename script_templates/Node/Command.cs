// meta-description: Base template for YAT commands.

using _BINDINGS_NAMESPACE_;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("_CLASS_", "Lorem ipsum dolor sit amet.", "[b]Usage[/b]: _CLASS_")]
	public partial class _CLASS_ : ICommand
	{
		public YAT Yat { get; set; }

		public _CLASS_(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(YAT yat, params string[] args)
		{
			yat.Terminal.Print("_CLASS_ is not yet implemented!", Terminal.PrintType.Warning);

			return CommandResult.Failure;
		}
	}
}
