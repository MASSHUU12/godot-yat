// meta-description: Base template for extensible YAT commands.

using _BINDINGS_NAMESPACE_;
using System;
using YAT.Attributes;
using YAT.Classes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("_CLASS_")]
[Description("Lorem ipsum dolor sit amet.")]
[Argument("action", "string", "The name of the action to run.")]
public partial class _CLASS_ : Extensible, ICommand
{
    public CommandResult Execute(CommandData data)
    {
        var extensions = GetCommandExtensions("_CLASS_");

        if (extensions is null) return ICommand.Failure("No extensions found.");

        if (extensions.TryGetValue((string)data.Arguments["action"], out Type extension))
            return ExecuteExtension(extension, data with { RawData = data.RawData[1..] });

        return ICommand.Failure("Variable not found.");
    }
}
