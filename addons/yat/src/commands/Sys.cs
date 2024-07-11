using System.Text;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("sys", "Runs a system command.")]
[Argument(
    "command",
    "string",
    "The command to run (if contains more than one word, you need to wrap it in the parentheses)."
)]
[Option("-program", "string", "The program to run the command with (default to systems specific terminal).", "")]
[Threaded]
public sealed class Sys : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        var program = (string)data.Options["-program"];
        var command = (string)data.Arguments["command"];
        var commandName = command.Split(' ')[0];
        var commandArgs = command[commandName.Length..].Trim() ?? string.Empty;

        StringBuilder result = OS.RunCommand(commandName, out var status, program, commandArgs);

        if (status == OS.ExecutionResult.Success)
        {
            data.Terminal.Output.Print(result.ToString());
        }
        else
        {
            data.Terminal.Output.Error(result.ToString());
        }

        return ICommand.Success();
    }
}
