using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes;
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
        CommandResult? result = null;

        foreach (ICommand command in _commands)
        {
            Dictionary<StringName, object> cA = [], cO = [];

            if (!ConvertInputData(
                command,
                context.Terminal.CommandValidator,
                result?.OutData ?? context.RawData[1..],
                ref cA,
                ref cO
            ))
            {
                return ICommand.InvalidArguments();
            }

            context = context with
            {
                Command = command,
                RawData = result?.OutData ?? context.RawData,
                Arguments = cA,
                Options = cO
            };

            context.Terminal.Locked = true;
            {
                result = command.GetAttribute<ThreadedAttribute>() is not null
                            ? await Task.Run(
                                () => command.Execute(context),
                                context.CancellationToken
                            )
                            : command.Execute(context);
            }
            context.Terminal.Locked = false;

            if (result.Result is not Success or Ok)
            {
                return result;
            }
        }

        return result!;
    }

    private static bool ConvertInputData(
        ICommand command,
        CommandValidator validator,
        string[] outputData,
        ref Dictionary<StringName, object> cA,
        ref Dictionary<StringName, object> cO
    )
    {
        if (command.GetAttribute<NoValidateAttribute>() is null)
        {
            if (!validator.ValidatePassedData<ArgumentAttribute>(
                command, outputData, out cA
            ))
            {
                return false;
            }

            if (command.GetAttributes<OptionAttribute>() is not null
                && !validator.ValidatePassedData<OptionAttribute>(
                    command, outputData, out cO
                ))
            {
                return false;
            }
        }

        return true;
    }
}
