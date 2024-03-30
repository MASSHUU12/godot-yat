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

		if (string.IsNullOrEmpty(_cache)) GenerateList(data.Terminal);

		return ICommand.Ok(_cache);
	}

	private static void GenerateList(BaseTerminal terminal)
	{
		StringBuilder sb = new();

		sb.AppendLine("Available commands:");

		var maxHeadSize = 0;
		foreach (var command in RegisteredCommands.Registered)
		{
			maxHeadSize = command.Key.Length > maxHeadSize ? command.Key.Length : maxHeadSize;
		}

		var linkStr = " - ";
		var tabSpaceNum = 2;
		var indentation = tabSpaceNum + maxHeadSize + linkStr.Length;
		foreach (var command in RegisteredCommands.Registered)
		{
			if (command.Value.GetCustomAttribute<CommandAttribute>() is not { } attribute) continue;

			var description = command.Value.GetCustomAttribute<DescriptionAttribute>();

			// Skip aliases
			if (attribute.Aliases.Contains(command.Key)) continue;
			var header = $"{new string(' ', tabSpaceNum)}[b]{command.Key}[/b]{new string(' ', maxHeadSize - command.Key.Length)} - ";
			sb.Append(header);
			var descriptionStr = description?.Description ?? attribute.Description;
			sb.Append(descriptionStr.Replace("\n", $"\n{new string(' ', indentation)}"));
			sb.AppendLine();
		}

		_cache = sb.ToString();
	}
}
