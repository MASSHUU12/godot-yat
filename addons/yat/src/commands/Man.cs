using System;
using YAT.Attributes;
using YAT.Classes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("man", "Displays the manual for a command.", "[b]Usage[/b]: man [i]command_name[/i]")]
[Argument("command_name", "string", "The name of the command to display the manual for.")]
[Option("-e", "bool", "Embed the manual in the terminal. Instead of opening in a new window.")]
public sealed class Man : ICommand
{
	private readonly LRUCache<string, string> cache = new(10);

	public CommandResult Execute(CommandData data)
	{
		var commandName = (string)data.Arguments["command_name"];
		var embed = (bool)data.Options["-e"];

		if (!RegisteredCommands.Registered.TryGetValue(commandName, out Type value))
			return ICommand.InvalidCommand(Messages.UnknownCommand(commandName));

		ICommand command = (ICommand)Activator.CreateInstance(value);

		// Check if the command manual is already in the cache.
		if (cache.Get(commandName) is string manual)
		{
			data.Terminal.Print(manual);
			return ICommand.Success();
		}

		manual = command.GenerateCommandManual();
		manual += command.GenerateArgumentsManual();
		manual += command.GenerateOptionsManual();
		manual += command.GenerateSignalsManual();

		if (command is Extensible extensible)
			manual += extensible.GenerateExtensionsManual();

		cache.Add(commandName, manual);

		if (embed) data.Terminal.Print(manual);
		else data.Terminal.FullWindowDisplay.Open(manual);

		return ICommand.Success();
	}
}
