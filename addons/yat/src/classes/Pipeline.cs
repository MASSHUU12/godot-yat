using System.Collections.Generic;
using Godot;
using YAT.Attributes;
using YAT.Classes.Managers;
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

    public CommandResult Execute(CommandData context)
    {
        CommandManager manager = context.Terminal.CommandManager;
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

            if (command.GetAttribute<ThreadedAttribute>() is not null)
            {
                // result = await ExecuteCommandInThreadAsync();

                // TODO: Resolve this
                return ICommand.Failure(
                    "Threads are broken in Godot, or I'm just stupid. "
                    + "Before I was able to work around this, but this no longer works."
                );
            }

            ExecuteCommand();

            context.Terminal.LastCommandResult = result!.Result;

            if (result!.Result is not Success or Ok)
            {
                return result;
            }
        }

        void ExecuteCommand()
        {
            _ = manager.CallDeferredThreadGroup(
                "emit_signal",
                "CommandStarted",
                nameof(context.Command),
                context.RawData
            );

            result = context.Command!.Execute(context);

            _ = manager.CallDeferredThreadGroup(
                "emit_signal",
                "CommandFinished",
                nameof(context.Command),
                context.RawData,
                (ushort)result.Result
            );
        }

        // async Task<CommandResult> ExecuteCommandInThreadAsync()
        // {
        //     TaskCompletionSource<CommandResult> tcs = new();
        //     GodotThread t = new();
        //     _ = t.Start(Callable.From(() =>
        //     {
        //         ExecuteCommand();
        //         tcs.SetResult(result!);
        //     }));

        //     return await tcs.Task;
        // }

        // async Task ExecuteCommandAsync()
        // {
        //     Task task = Task.Run(() => ExecuteCommand(), context.CancellationToken);
        //     task.Wait();
        //     // new Task(() => ExecuteCommand(), context.CancellationToken).Start();
        //     // await Task.Run(() => ExecuteCommand(), context.CancellationToken);
        //     _ = await manager.ToSignal(manager, "CommandFinished");
        // }

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
