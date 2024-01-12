// meta-description: Base template for YAT commands extension.

using _BINDINGS_NAMESPACE_;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

[Extension("_CLASS_", "Lorem ipsum dolor sit amet.", "[b]Usage[/b]: _CLASS_")]
public partial class _CLASS_ : IExtension
{
	public CommandResult Execute(CommandArguments args)
	{
		args.Terminal.Print("_CLASS_ is not yet implemented!", PrintType.Error);

		return CommandResult.Failure;
	}
}
