using System;
using System.Linq;
using System.Text;
using YAT.Attributes;

namespace YAT.Commands
{
	[Command("list", "List all available commands", "[b]Usage[/b]: list", "ls")]
	public partial class List : ICommand
	{
		public YAT Yat { get; set; }

		public List(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			var sb = new StringBuilder();

			sb.AppendLine("Available commands:");

			foreach (var command in Yat.Commands)
			{
				if (Attribute.GetCustomAttribute(command.Value.GetType(), typeof(CommandAttribute)) is not CommandAttribute attribute) continue;

				// Skip aliases
				if (attribute.Aliases.Contains(command.Key)) continue;

				sb.Append($"[b]{command.Key}[/b] - ");
				sb.Append(attribute.Description);
				sb.AppendLine();
			}

			Yat.Terminal.Print(sb.ToString());

			return CommandResult.Success;
		}
	}
}
