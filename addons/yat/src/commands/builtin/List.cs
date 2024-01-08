using System;
using System.Linq;
using System.Text;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("list", "List all available commands", "[b]Usage[/b]: list", "lc")]
	public partial class List : ICommand
	{
		public CommandResult Execute(CommandArguments args)
		{
			var sb = new StringBuilder();

			sb.AppendLine("Available commands:");

			foreach (var command in args.Yat.Commands)
			{
				if (Attribute.GetCustomAttribute(command.Value.GetType(), typeof(CommandAttribute)) is not CommandAttribute attribute) continue;

				// Skip aliases
				if (attribute.Aliases.Contains(command.Key)) continue;

				sb.Append($"[b]{command.Key}[/b] - ");
				sb.Append(attribute.Description);
				sb.AppendLine();
			}

			args.Terminal.Print(sb.ToString());

			return CommandResult.Success;
		}
	}
}
