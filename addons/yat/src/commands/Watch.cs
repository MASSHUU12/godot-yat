using System.Threading;
using YAT.Attributes;
using YAT.Classes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Threaded]
[Command("watch")]
[Description("Runs user-defined commands at regular intervals." +
    "\nNote: Not threaded commands might not work as expected."
)]
[Argument("command", "string", "The command to run.")]
[Option("--interval", "float(0.5:60)", "The interval at which to run the command.", 1f)]
public sealed class Watch : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        var parsed = Parser.ParseCommand((string)data.Arguments["command"]);
        var commandName = parsed[0];

        if (!RegisteredCommands.Registered.TryGetValue(commandName, out var type))
        {
            return ICommand.InvalidArguments(
                $"Command '{commandName}' not found, exiting watch."
            );
        }

        if (type.Name == nameof(Watch))
        {
            return ICommand.Failure(
                "Cannot watch the watch command, exiting watch."
            );
        }

        var interval = (float)data.Options["--interval"] * 1000;

        while (!data.CancellationToken.IsCancellationRequested)
        {
            if (!data.Terminal.CommandManager.Run(parsed, data.Terminal))
            {
                return ICommand.Failure(
                    $"Error executing command '{commandName}', exiting watch."
                );
            }

            Thread.Sleep((int)interval);
        }

        return ICommand.Success();
    }
}
