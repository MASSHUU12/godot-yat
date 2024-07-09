// meta-description: Base template for YAT commands extension.

using _BINDINGS_NAMESPACE_;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

[Extension("_CLASS_")]
[Description("Lorem ipsum dolor sit amet.")]
public sealed class _CLASS_ : IExtension
{
    public CommandResult Execute(CommandData data)
    {
        return CommandResult.NotImplemented("_CLASS_ is not yet implemented!");
    }
}
