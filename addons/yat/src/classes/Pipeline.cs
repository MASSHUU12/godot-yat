using System.Collections.Generic;
using System.Threading.Tasks;
using YAT.Attributes;
using YAT.Helpers;
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

    public void Clear()
    {
        _commands.Clear();
    }

    public async Task<CommandResult> ExecuteAsync(CommandData context)
    {
        CommandResult? result = ICommand.Ok();

        foreach (ICommand command in _commands)
        {
            context.Terminal.Locked = true;
            result = command.GetAttribute<ThreadedAttribute>() is not null
                ? await Task.Run(() => command.Execute(context))
                : command.Execute(context);
            context.Terminal.Locked = false;

            if (result.Result is not Success or Ok)
            {
                return result;
            }
        }

        return result;
    }
}
