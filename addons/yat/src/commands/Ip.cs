using System.Globalization;
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
        string action = (string)data.Arguments["action"];

        if (action == "addr")
        {
            return ICommand.Success([PrintLocalInterfaces(data.Terminal)]);
        }

        return ICommand.Success();
    }

    private static string PrintLocalInterfaces(BaseTerminal terminal)
    {
        StringBuilder sb = new();
        Array<Dictionary> interfaces = IP.GetLocalInterfaces();

        if (interfaces.Count == 0)
        {
            _ = sb.AppendLine("No local interfaces found.");
        }

        foreach (Dictionary? iface in interfaces)
        {
            _ = sb.AppendLine(
                CultureInfo.InvariantCulture,
                $"[b]{iface["index"]}[/b]: {iface["friendly"]} ({iface["name"]})"
            );
            _ = sb.AppendLine("Addresses:");

            foreach (string addr in iface["addresses"].AsStringArray())
            {
                _ = sb.AppendLine(CultureInfo.InvariantCulture, $"\t{addr}");
            }

            _ = sb.AppendLine();
        }

        terminal.Print(sb.ToString());
        return sb.ToString();
    }
}
