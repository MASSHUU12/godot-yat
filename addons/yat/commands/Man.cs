using System;
using System.Text;

namespace YAT.Commands
{
	[Command("man", "Displays the manual for a command.", "[b]Usage[/b]: man [i]command_name[/i]")]
	public partial class Man : ICommand
	{
		public void Execute(YAT yat, params string[] args)
		{
			if (args.Length < 2)
			{
				yat.Terminal.Println("Invalid input.");
				return;
			}

			var lookup = yat.Commands;
			var commandName = args[1];

			if (lookup.ContainsKey(commandName))
			{
				var command = lookup[commandName];
				CommandAttribute attribute = Attribute.GetCustomAttribute(command.GetType(), typeof(CommandAttribute)) as CommandAttribute;
				StringBuilder sb = new();

				sb.AppendLine($"[p align=center][font_size=22]{attribute.Name}[/font_size][/p]");
				sb.AppendLine($"[p align=center]{attribute.Description}[/p]");
				sb.AppendLine(attribute.Manual);
				sb.AppendLine("\n[b]Aliases[/b]:");

				if (attribute.Aliases.Length > 0)
				{
					foreach (var alias in attribute.Aliases)
						sb.Append($"[ul]\t{alias}[/ul]");
				}
				else sb.AppendLine("[ul]\tNone[/ul]");

				yat.Terminal.Println(sb.ToString());
			}
			else yat.Terminal.Println($"Unknown command: {commandName}");
		}
	}
}
