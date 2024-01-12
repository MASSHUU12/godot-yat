// meta-description: Base template for extensible YAT commands.

using _BINDINGS_NAMESPACE_;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

[Command("_CLASS_", "Lorem ipsum dolor sit amet.", "[b]Usage[/b]: _CLASS_")]
public partial class _CLASS_ : Extensible, ICommand
{
	public CommandResult Execute(CommandArguments args)
	{
		var option = args.Arguments[1];

		if (Extensions.ContainsKey(option))
		{
			var extension = Extensions[option];
			return extension.Execute(args with { Arguments = args.Arguments[1..] });
		}

		args.Terminal.Println("Options not found.", PrintType.Error);
		return CommandResult.Failure;
	}
}
