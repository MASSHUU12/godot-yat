using System.Text;

public partial class Man : IYatCommand
{
    public string Name => "man";

    public string Description => "Displays the manual for a command.";

    public string Usage => "man <command_name>";

    public string[] Aliases => System.Array.Empty<string>();

    public void Execute(string[] args, YAT yat)
    {
        var lookup = yat.Commands;

        if (args.Length < 2)
        {
            yat.Terminal.Println("Invalid input.");
            return;
        }

        var commandName = args[1];

        if (lookup.ContainsKey(commandName))
        {
            var command = lookup[commandName];
            StringBuilder sb = new();

            sb.AppendLine($"[p align=center][font_size=22]{command.Name}[/font_size][/p]");
            sb.AppendLine($"[p align=center]{command.Description}[/p]");
            sb.AppendLine($"[b]Usage[/b]: {command.Usage}");
            sb.AppendLine("[b]Aliases[/b]:");

            if (command.Aliases.Length > 0)
            {
                foreach (var alias in command.Aliases)
                    sb.Append($"[ul]\t{alias}[/ul]");
            }
            else sb.AppendLine("[ul]\tNone[/ul]");

            yat.Terminal.Println(sb.ToString());
        }
        else yat.Terminal.Println($"Unknown command: {commandName}");
    }
}
