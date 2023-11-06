using System;
using System.Linq;
using System.Text;

namespace YAT.Commands
{
	[Command("list", "List all available commands", "[b]Usage[/b]: list", "ls")]
	public partial class List : ICommand
	{
		public CommandResult Execute(YAT yat, params string[] args)
		{
			var sb = new StringBuilder();

			sb.AppendLine("Available commands:");

			foreach (var command in yat.Commands)
			{
				if (Attribute.GetCustomAttribute(command.Value.GetType(), typeof(CommandAttribute)) is not CommandAttribute attribute) continue;

				// Skip aliases
				if (attribute.Aliases.Contains(command.Key)) continue;

				sb.Append($"[b]{command.Key}[/b] - ");
				sb.Append(attribute.Description);
				sb.AppendLine();
			}

			yat.Terminal.Println(sb.ToString());

			return CommandResult.Success;
		}
	}
}
