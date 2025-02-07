using System.Text;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;
using static YAT.Helpers.OS;

namespace YAT.Commands;

[Command("sys", "Runs a system command.")]
[Argument(
    "command",
    "string",
    "The command to run (if contains more than one word, you need to wrap it in "
    + "the parentheses)."
)]
[Option("-program", "string",
    "The program to run the command with (default to systems specific terminal).",
    ""
)]
[Threaded]
public sealed class Sys : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        string program = (string)data.Options["-program"];
        string command = (string)data.Arguments["command"];
        string commandName = command.Split(' ')[0];
        string commandArgs = command[commandName.Length..].Trim() ?? string.Empty;

        StringBuilder result = RunCommand(
            commandName,
            out EExecutionResult status,
            program,
            commandArgs
        );

        if (status == EExecutionResult.Success)
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
