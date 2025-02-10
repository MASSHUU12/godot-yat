using System;
using System.Globalization;
using System.Linq;
using System.Text;
using YAT.Attributes;
using YAT.Debug;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("ds", "Displays items in the debug screen.")]
[Argument("screens", "stop|all|string", "Debug screen/s to display.")]
[Option("-h", "bool", "Displays this help message.")]
[Option("--interval", "float(0.05:)", "Update interval.", 0f)]
public sealed class Ds : ICommand
{
    private static DebugScreen? _debug;

    public CommandResult Execute(CommandData data)
    {
        bool showHelp = (bool)data.Options["-h"];
        string screens = (string)data.Arguments["screens"];
        float interval = (float)data.Options["--interval"];

        _debug ??= data.Yat.GetTree().Root.GetNode<DebugScreen>("/root/DebugScreen");

        _debug.UpdateInterval = interval == 0f
            ? DebugScreen.DefaultUpdateInterval
            : interval;

        if (showHelp)
        {
            return ICommand.Ok(message: Help());
        }

        switch (screens.ToLowerInvariant())
        {
            case "all":
                _debug.RunAll();
                break;
            case "stop":
                _debug.RunSelected();
                break;
            default:
                _debug.RunSelected(screens.Split(",", StringSplitOptions.TrimEntries));
                break;
        }

        return ICommand.Success();
    }

    private static string Help()
    {
        StringBuilder message = new("Registered debug items:\n");

        foreach (Type type in DebugScreen.RegisteredItems.Values
            .SelectMany(static x => x)
        )
        {
            string title = type.GetAttribute<TitleAttribute>()?.Title ?? type.Name;
            _ = message.AppendLine(
                CultureInfo.InvariantCulture,
                $"- [b]{title}[/b]: {type.Name}"
            );
        }

        return message.ToString();
    }
}
