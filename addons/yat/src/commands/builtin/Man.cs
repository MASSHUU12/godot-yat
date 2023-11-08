using System.Linq;
using System.Text;
using YAT.Helpers;

namespace YAT.Commands
{
	[Command("man", "Displays the manual for a command.", "[b]Usage[/b]: man [i]command_name[/i]")]
	public partial class Man : ICommand
	{
		// TODO: Cache the manual for each command so we don't have to re-parse it every time.

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

			PrintManual(yat, attribute);

			if (command is Extensible extensible) PrintExtensions(yat, extensible);

			return CommandResult.Success;
		}

		/// <summary>
		/// Prints the manual for a given command.
		/// </summary>
		/// <param name="yat">The YAT instance.</param>
		/// <param name="command">The CommandAttribute of the command to print the manual for.</param>
		private static void PrintManual(YAT yat, CommandAttribute command)
		{
			StringBuilder sb = new();

			sb.AppendLine($"[p align=center][font_size=22]{command.Name}[/font_size][/p]");
			sb.AppendLine($"[p align=center]{command.Description}[/p]");
			sb.AppendLine(command.Manual);
			sb.AppendLine("\n[b]Aliases[/b]:");
			sb.AppendLine(command.Aliases.Length > 0
					? string.Join("\n", command.Aliases.Select(alias => $"[ul]\t{alias}[/ul]"))
					: "[ul]\tNone[/ul]");

			yat.Terminal.Print(sb.ToString());
		}

		/// <summary>
		/// Prints the extensions of the given <see cref="Extensible"/> object to the terminal.
		/// </summary>
		/// <param name="yat">The <see cref="YAT"/> instance.</param>
		/// <param name="extensible">The <see cref="Extensible"/> object whose extensions to print.</param>
		private static void PrintExtensions(YAT yat, Extensible extensible)
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

			yat.Terminal.Print(sb.ToString());
		}
	}
}
