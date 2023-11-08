using System.Linq;
using System.Text;
using YAT.Helpers;

namespace YAT.Commands
{
	[Command("man", "Displays the manual for a command.", "[b]Usage[/b]: man [i]command_name[/i]")]
	public partial class Man : ICommand
	{
		private readonly LRUCache<string, string> cache = new(10);

		public CommandResult Execute(YAT yat, params string[] args)
		{
			if (args.Length < 2)
			{
				LogHelper.MissingArguments("man", "command_name");
				return CommandResult.InvalidArguments;
			}

			var commandName = args[1];

			if (!yat.Commands.TryGetValue(commandName, out var command))
			{
				LogHelper.UnknownCommand(commandName);
				return CommandResult.InvalidCommand;
			}

			CommandAttribute attribute = command.GetAttribute<CommandAttribute>();

			if (attribute is null || string.IsNullOrEmpty(attribute.Manual))
			{
				yat.Terminal.Print($"Command {commandName} does not have a manual.");
				return CommandResult.Failure;
			}

			// Check if the manual is already cached
			if (cache.Get(commandName) is string manual)
			{
				yat.Terminal.Print(manual);
				return CommandResult.Success;
			}

			manual = PrintManual(yat, attribute);

			if (command is Extensible extensible)
				manual += PrintExtensions(yat, extensible);

			cache.Add(commandName, manual);

			return CommandResult.Success;
		}

		/// <summary>
		/// Prints the manual for a given command.
		/// </summary>
		/// <param name="yat">The YAT instance.</param>
		/// <param name="command">The CommandAttribute of the command to print the manual for.</param>
		/// <returns>The manual as a string.</returns>
		private static string PrintManual(YAT yat, CommandAttribute command)
		{
			StringBuilder sb = new();

			sb.AppendLine($"[p align=center][font_size=22]{command.Name}[/font_size][/p]");
			sb.AppendLine($"[p align=center]{command.Description}[/p]");
			sb.AppendLine(command.Manual);
			sb.AppendLine("\n[b]Aliases[/b]:");
			sb.AppendLine(command.Aliases.Length > 0
					? string.Join("\n", command.Aliases.Select(alias => $"[ul]\t{alias}[/ul]"))
					: "[ul]\tNone[/ul]");

			var manual = sb.ToString();

			yat.Terminal.Print(manual);

			return manual;
		}

		/// <summary>
		/// Prints the extensions of the given <see cref="Extensible"/> object to the terminal.
		/// </summary>
		/// <param name="yat">The <see cref="YAT"/> instance.</param>
		/// <param name="extensible">The <see cref="Extensible"/> object whose extensions to print.</param>
		/// <returns>The extensions as a string.</returns>
		private static string PrintExtensions(YAT yat, Extensible extensible)
		{
			StringBuilder sb = new();

			sb.AppendLine("[p align=center][font_size=22]Extensions[/font_size][/p]");

			foreach (var extension in extensible.Extensions)
			{
				var attribute = extension.Value.GetAttribute<ExtensionAttribute>();

				sb.AppendLine($"[font_size=18]{attribute.Name}[/font_size]");
				sb.AppendLine(attribute.Description);
				sb.AppendLine('\n' + attribute.Manual);
				sb.AppendLine("\n[b]Aliases[/b]:");
				sb.AppendLine(attribute.Aliases.Length > 0
						? string.Join("\n", attribute.Aliases.Select(alias => $"[ul]\t{alias}[/ul]"))
						: "[ul]\tNone[/ul]");
			}

			var extensions = sb.ToString();

			yat.Terminal.Print(extensions);

			return extensions;
		}
	}
}
