using System;
using System.Linq;
using System.Reflection;
using System.Text;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("list", "List all available commands", "[b]Usage[/b]: list", "lc")]
[Option("-f", "bool", "Flush the cache before listing commands.")]
public sealed class List : ICommand
{
	private static string _cache = string.Empty;

	public CommandResult Execute(CommandData data)
	{
		if ((bool)data.Options["-f"]) _cache = string.Empty;

		if (string.IsNullOrEmpty(_cache)) PrintList(data.Terminal);
		else data.Terminal.Print(_cache);

		return ICommand.Success();
	}

	private static void PrintList(BaseTerminal terminal)
	{
		var sb = new StringBuilder();

		sb.AppendLine("Available commands:");

		foreach (var command in RegisteredCommands.Registered)
		{
			if (command.Value.GetCustomAttribute<CommandAttribute>() is not CommandAttribute attribute) continue;

			var description = command.Value.GetCustomAttribute<DescriptionAttribute>();

			// Skip aliases
			if (attribute.Aliases.Contains(command.Key)) continue;

			sb.Append($"[b]{command.Key}[/b] - ");
			sb.Append(description?.Description ?? attribute.Description);
			sb.AppendLine();
		}

		_cache = sb.ToString();
		terminal.Print(_cache);
	}
}
