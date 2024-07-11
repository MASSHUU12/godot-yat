using System;
using System.Text;
using YAT.Attributes;
using YAT.Classes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("man", "Displays the manual for a command.")]
[Argument("command_name", "string", "The name of the command to display the manual for.")]
[Option("-e", "bool", "Embed the manual in the terminal. Instead of opening in a new window.")]
public sealed class Man : ICommand
{
    private readonly LRUCache<string, string> cache = new(10);

    public CommandResult Execute(CommandData data)
    {
        var commandName = (string)data.Arguments["command_name"];
        var embed = (bool)data.Options["-e"];

        if (!RegisteredCommands.Registered.TryGetValue(commandName, out var value))
        {
            return ICommand.InvalidCommand(Messages.UnknownCommand(commandName));
        }

        ICommand command = (ICommand)Activator.CreateInstance(value)!;

        // Check if the command manual is already in the cache.
        if (cache.Get(commandName) is string manual)
        {
            if (embed)
            {
                data.Terminal.Print(manual);
            }
            else
            {
                data.Terminal.FullWindowDisplay.Open(manual);
            }

            return ICommand.Success();
        }

        StringBuilder bManual = command.GenerateCommandManual();
        _ = bManual.Append(command.GenerateArgumentsManual())
            .Append(command.GenerateOptionsManual())
            .Append(command.GenerateSignalsManual());

        if (command is Extensible extensible)
        {
            _ = bManual.Append(extensible.GenerateExtensionsManual());
        }

        cache.Add(commandName, bManual.ToString());

        if (embed)
        {
            data.Terminal.Print(bManual);
        }
        else
        {
            data.Terminal.FullWindowDisplay.Open(bManual.ToString());
        }

        return ICommand.Success();
    }
}
