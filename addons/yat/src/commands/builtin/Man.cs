using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("man", "Displays the manual for a command.", "[b]Usage[/b]: man [i]command_name[/i]")]
	[Argument("command_name", "string", "The name of the command to display the manual for.")]
	public partial class Man : ICommand
	{
		private readonly LRUCache<string, string> cache = new(10);

		public CommandResult Execute(CommandArguments args)
		{
			var commandName = args.Arguments[1];

			if (!args.Yat.Commands.TryGetValue(commandName, out var command))
			{
				LogHelper.UnknownCommand(commandName);
				return CommandResult.InvalidCommand;
			}

			// Check if the command manual is already in the cache.
			if (cache.Get(commandName) is string manual)
			{
				args.Terminal.Print(manual);
				return CommandResult.Success;
			}

			manual = command.GenerateCommandManual();
			manual += command.GenerateArgumentsManual();
			manual += command.GenerateOptionsManual();
			manual += command.GenerateSignalsManual();

			if (command is Extensible extensible)
				manual += extensible.GenerateExtensionsManual();

			cache.Add(commandName, manual);

			args.Terminal.Print(manual);

			return CommandResult.Success;
		}
	}
}
