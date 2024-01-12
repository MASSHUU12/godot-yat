// meta-description: Base template for YAT commands.

using _BINDINGS_NAMESPACE_;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Commands
{
	[Command("_CLASS_", "Lorem ipsum dolor sit amet.", "[b]Usage[/b]: _CLASS_")]
	public partial class _CLASS_ : ICommand
	{
		public CommandResult Execute(CommandArguments args)
		{
			args.Terminal.Print("_CLASS_ is not yet implemented!", PrintType.Warning);

			return CommandResult.Failure;
		}
	}
}
