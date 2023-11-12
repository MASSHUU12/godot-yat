// meta-description: Base template for YAT commands extension.

using _BINDINGS_NAMESPACE_;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

[Extension("_CLASS_", "Lorem ipsum dolor sit amet.", "[b]Usage[/b]: _CLASS_")]
public partial class _CLASS_ : IExtension
{
	public CommandResult Execute(YAT.YAT yat, ICommand command, params string[] args)
	{
		yat.Terminal.Print("_CLASS_ is not yet implemented!", Terminal.PrintType.Warning);

		return CommandResult.Failure;
	}
}
