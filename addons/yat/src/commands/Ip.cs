using System.Text;
using Godot;
using Godot.Collections;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("ip")]
[Description("Displays information about the local network interfaces.")]
[Argument("action", "addr", "The action to perform.")]
public sealed class Ip : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        var action = (string)data.Arguments["action"];

        if (action == "addr")
        {
            PrintLocalInterfaces(data.Terminal);
        }

        return ICommand.Success();
    }

    private static void PrintLocalInterfaces(BaseTerminal terminal)
    {
        StringBuilder sb = new();
        var interfaces = IP.GetLocalInterfaces();

        if (interfaces.Count == 0)
        {
            terminal.Print("No local interfaces found.");
            return;
        }

        foreach (Dictionary? iface in interfaces)
        {
            _ = sb.AppendLine($"[b]{iface["index"]}[/b]: {iface["friendly"]} ({iface["name"]})");
            _ = sb.AppendLine("Addresses:");

            foreach (string addr in iface["addresses"].AsStringArray())
            {
                _ = sb.AppendLine($"\t{addr}");
            }

            _ = sb.AppendLine();
        }

        terminal.Print(sb.ToString());
    }
}
