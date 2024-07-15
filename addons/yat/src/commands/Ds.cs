using System;
using System.Linq;
using System.Text;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes;
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
        var showHelp = (bool)data.Options["-h"];
        var screens = (string)data.Arguments["screens"];
        var interval = (float)data.Options["--interval"];

        _debug ??= data.Yat.GetTree().Root.GetNode<DebugScreen>("/root/DebugScreen");

        _debug.UpdateInterval = interval == 0f
            ? _debug.DefaultUpdateInterval
            : interval;

        if (showHelp)
        {
            return ICommand.Success(Help());
        }

        switch (screens.ToLower())
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

        foreach (var (uid, type) in DebugScreen.registeredItems.Values.SelectMany(x => x))
        {
            var title = type.GetAttribute<TitleAttribute>()?.Title ?? type.Name;
            _ = message.AppendFormat(
                "- [b]{0}[/b] ({1}): {2}\n",
                title,
                type.Name == title ? string.Empty : type.Name,
                uid
            );
        }

        return message.ToString();
    }
}
