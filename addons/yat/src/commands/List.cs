using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("list", "List all available commands")]
[Option("-f", "bool", "Flush the cache before listing commands.")]
public sealed class List : ICommand
{
    private static string _cache = string.Empty;

    public CommandResult Execute(CommandData data)
    {
        if ((bool)data.Options["-f"])
        {
            _cache = string.Empty;
        }

        if (string.IsNullOrEmpty(_cache))
        {
            GenerateList();
        }

        return ICommand.Ok([_cache], _cache);
    }

    private static void GenerateList()
    {
        StringBuilder sb = new();

        _ = sb.AppendLine("Available commands:");

        int maxHeadSize = 0;
        foreach (KeyValuePair<string, Type> command in RegisteredCommands.Registered)
        {
            maxHeadSize = command.Key.Length > maxHeadSize
                ? command.Key.Length
                : maxHeadSize;
        }

        const string linkStr = " - ";
        const int tabSpaceNum = 2;
        int indentation = tabSpaceNum + maxHeadSize + linkStr.Length;
        foreach (KeyValuePair<string, Type> command in RegisteredCommands.Registered)
        {
            if (command.Value.GetCustomAttribute<CommandAttribute>() is not { } attribute)
            {
                continue;
            }

            DescriptionAttribute? description = command.Value
                .GetCustomAttribute<DescriptionAttribute>();

            if (attribute.Aliases.Contains(command.Key))
            {
                continue;
            }

            string header = string.Format(
                CultureInfo.InvariantCulture,
                "{0}[b]{1}[/b]{2} - ",
                new string(' ', tabSpaceNum),
                command.Key,
                new string(' ', maxHeadSize - command.Key.Length)
            );
            _ = sb.Append(header);

            string descriptionStr = description?.Description ?? attribute.Description;
            _ = sb.Append(descriptionStr.Replace("\n", $"\n{new string(' ', indentation)}"));
            _ = sb.AppendLine();
        }

        _cache = sb.ToString();
    }
}
