// meta-description: Base template for extensible YAT commands.

using _BINDINGS_NAMESPACE_;
using System;
using YAT.Attributes;
using YAT.Classes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Types;

[Command("_CLASS_", "Lorem ipsum dolor sit amet.", "[b]Usage[/b]: _CLASS_ [i]action[/i]")]
[Argument("action", "string", "The name of the action to run.")]
public partial class _CLASS_ : Extensible, ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var extensions = GetCommandExtensions("_CLASS_");

		if (extensions.TryGetValue((string)data.Arguments["action"], out Type extension))
			return ExecuteExtension(extension, data with { RawData = data.RawData[1..] });

		data.Terminal.Print("Action not found.", EPrintType.Error);
		return CommandResult.Failure;
	}
}
