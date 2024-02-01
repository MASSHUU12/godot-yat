// meta-description: Base template for extensible YAT commands.

using _BINDINGS_NAMESPACE_;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Types;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

[Command("_CLASS_", "Lorem ipsum dolor sit amet.", "[b]Usage[/b]: _CLASS_ [i]action[/i]")]
public partial class _CLASS_ : Extensible, ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var option = data.Arguments[1];

		if (Extensions.ContainsKey(option))
		{
			var extension = Extensions[option];
			return extension.Execute(data with { Arguments = data.Arguments[1..] });
		}

		data.Terminal.Print("Options not found.", PrintType.Error);
		return CommandResult.Failure;
	}
}
