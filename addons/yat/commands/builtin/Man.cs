using System.Linq;
using System.Text;
using YAT.Helpers;

namespace YAT.Commands
{
	[Command("man", "Displays the manual for a command.", "[b]Usage[/b]: man [i]command_name[/i]")]
	public partial class Man : ICommand
	{
		public CommandResult Execute(YAT yat, params string[] args)
		{
			if (args.Length < 2)
			{
				yat.Terminal.Println("Invalid input.");
				return CommandResult.InvalidArguments;
			}

			var commandName = args[1];

			if (!yat.Commands.TryGetValue(commandName, out var command))
			{
				yat.Terminal.Println($"Unknown command: {commandName}");
				return CommandResult.InvalidCommand;
			}

			CommandAttribute attribute = AttributeHelper.GetAttribute<CommandAttribute>(command);

			if (attribute is null || string.IsNullOrEmpty(attribute.Manual))
			{
				yat.Terminal.Println($"Command {commandName} does not have a manual.");
				return CommandResult.Failure;
			}

			PrintManual(yat, attribute);

			return CommandResult.Success;
		}

		/// <summary>
		/// Prints the manual for a given command.
		/// </summary>
		/// <param name="yat">The YAT instance.</param>
		/// <param name="attribute">The CommandAttribute of the command to print the manual for.</param>
		private static void PrintManual(YAT yat, CommandAttribute attribute)
		{
			StringBuilder sb = new();

			sb.AppendLine($"[p align=center][font_size=22]{attribute.Name}[/font_size][/p]");
			sb.AppendLine($"[p align=center]{attribute.Description}[/p]");
			sb.AppendLine(attribute.Manual);
			sb.AppendLine("\n[b]Aliases[/b]:");
			sb.AppendLine(attribute.Aliases.Length > 0
					? string.Join("\n", attribute.Aliases.Select(alias => $"[ul]\t{alias}[/ul]"))
					: "[ul]\tNone[/ul]");

			yat.Terminal.Println(sb.ToString());
		}
	}
}
