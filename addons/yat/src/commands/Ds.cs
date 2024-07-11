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
    public CommandResult Execute(CommandData data)
    {
        var h = (bool)data.Options["-h"];
        var i = ((object[])data.Options["-i"]).Cast<string>().ToArray();
        var interval = (float)data.Options["--interval"];

        data.Yat.DebugScreen.UpdateInterval = interval == 0f
            ? data.Yat.DebugScreen.DefaultUpdateInterval
            : interval;

        if (h)
        {
            Help(data.Terminal);
            return ICommand.Success();
        }

        if (i.Contains("all"))
        {
            data.Yat.DebugScreen.RunAll();
        }
        else
        {
            data.Yat.DebugScreen.RunSelected(i);
        }

        return ICommand.Success();
    }

    private static void Help(BaseTerminal terminal)
    {
        StringBuilder message = new();

        _ = message.AppendLine("Registered items:");

        foreach (var item in DebugScreen.registeredItems.Values.SelectMany(x => x))
        {
            _ = message.AppendFormat(
                    "- [b]{0}[/b] ({1}): {2}",
                    item.Item2.GetAttribute<TitleAttribute>()?.Title ?? item.Item2.Name,
                    item.Item2.Name,
                    item.Item1
                )
                .AppendLine();
        }

        terminal.Print(message);
    }
}
