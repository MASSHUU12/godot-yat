using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Commands;

[Command("set", "Sets a variable to a value.", "[b]Usage[/b]: set [i]variable[/i] [i]value[/i]")]
[Argument("variable", "string", "The name of the variable to set.")]
[Argument("value", "string", "The value to set the variable to.")]
public partial class Set : Extensible, ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var variable = data.RawData[1];

		if (Extensions.ContainsKey(variable))
		{
			var extension = Extensions[variable];
			return extension.Execute(data with { RawData = data.RawData[1..] });
		}

		data.Terminal.Print("Variable not found.", PrintType.Error);
		return CommandResult.Failure;
	}
}
