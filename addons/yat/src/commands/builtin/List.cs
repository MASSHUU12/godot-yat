using System;
using System.Linq;
using System.Reflection;
using System.Text;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("list", "List all available commands", "[b]Usage[/b]: list", "lc")]
	public partial class List : ICommand
	{
		public CommandResult Execute(CommandData data)
		{
			var sb = new StringBuilder();

			sb.AppendLine("Available commands:");

			foreach (var command in data.Yat.CommandManager.Commands)
			{
				if (command.Value.GetCustomAttribute<CommandAttribute>() is not CommandAttribute attribute) continue;

				// Skip aliases
				if (attribute.Aliases.Contains(command.Key)) continue;

				sb.Append($"[b]{command.Key}[/b] - ");
				sb.Append(attribute.Description);
				sb.AppendLine();
			}

			data.Terminal.Print(sb.ToString());

			return CommandResult.Success;
		}
	}
}
