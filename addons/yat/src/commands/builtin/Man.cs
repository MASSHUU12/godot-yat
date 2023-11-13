using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("man", "Displays the manual for a command.", "[b]Usage[/b]: man [i]command_name[/i]")]
	[Arguments("command_name:string")]
	public partial class Man : ICommand
	{
		private readonly LRUCache<string, string> cache = new(10);

		public YAT Yat { get; set; }

		public Man(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			var commandName = args[1];

			if (!Yat.Commands.TryGetValue(commandName, out var command))
			{
				LogHelper.UnknownCommand(commandName);
				return CommandResult.InvalidCommand;
			}

			// Check if the command manual is already in the cache.
			if (cache.Get(commandName) is string manual)
			{
				Yat.Terminal.Print(manual);
				return CommandResult.Success;
			}

			manual = command.GenerateCommandManual();

			if (command is Extensible extensible)
				manual += extensible.GenerateExtensionsManual();

			cache.Add(commandName, manual);

			Yat.Terminal.Print(manual);

			return CommandResult.Success;
		}
	}
}
