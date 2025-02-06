using System.Collections.Generic;
using YAT.Interfaces;
using YAT.Types;

using static YAT.Enums.ECommandResult;

namespace YAT.Classes;

public class Pipeline
{
    private readonly List<ICommand> _commands;

    public Pipeline()
    {
        _commands = [];
    }

    public void AddCommand(ICommand command)
    {
        _commands.Add(command);
    }

    public CommandResult Execute(CommandData context)
    {
        CommandResult? result = ICommand.Ok();

        foreach (ICommand command in _commands)
        {
            result = command.Execute(context);

            if (result.Result is not Success or Ok)
            {
                return result;
            }
        }

        return result;
    }
}
