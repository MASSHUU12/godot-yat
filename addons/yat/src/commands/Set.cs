using System;
using YAT.Attributes;
using YAT.Classes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("set", "Sets a variable to a value.", "[b]Usage[/b]: set [i]variable[/i] [i]value[/i]")]
[Argument("variable", "string", "The name of the variable to set.")]
[Argument("value", "string", "The value to set the variable to.")]
public partial class Set : Extensible, ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var extensions = GetCommandExtensions("set");

		if (extensions is null) return ICommand.Failure("No extensions found.");

		if (extensions.TryGetValue((string)data.Arguments["variable"], out var extension))
			return ExecuteExtension(extension, data with { RawData = data.RawData[1..] });

		return ICommand.Failure("Variable not found.");
	}
}
