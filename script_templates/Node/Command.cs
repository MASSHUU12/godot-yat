// meta-description: Base template for YAT commands.

using _BINDINGS_NAMESPACE_;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("_CLASS_")]
[Description("Lorem ipsum dolor sit amet.")]
public sealed class _CLASS_ : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        return CommandResult.NotImplemented("_CLASS_ is not yet implemented!");
    }
}
