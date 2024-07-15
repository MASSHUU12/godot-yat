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
[Option("-h", "bool", "Displays this help message.")]
[Option("-i", "string...", "Items to display.", new string[] { })]
[Option("--interval", "float(0.05:5)", "Update interval.", 0f)]
public sealed class Ds : ICommand
{
    private static DebugScreen? _debug;

    public CommandResult Execute(CommandData data)
    {
        var h = (bool)data.Options["-h"];
        var i = ((object[])data.Options["-i"]).Cast<string>().ToArray();
        var interval = (float)data.Options["--interval"];

        _debug ??= data.Yat.GetTree().Root.GetNode<DebugScreen>("/root/DebugScreen");

        _debug.UpdateInterval = interval == 0f
            ? _debug.DefaultUpdateInterval
            : interval;

        if (h)
        {
            Help(data.Terminal);
            return ICommand.Success();
        }

        if (i.Contains("all"))
        {
            _debug.RunAll();
        }
        else
        {
            _debug.RunSelected(i);
        }

        return ICommand.Success();
    }

    private static void Help(BaseTerminal terminal)
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

        terminal.Print(message);
    }
}
