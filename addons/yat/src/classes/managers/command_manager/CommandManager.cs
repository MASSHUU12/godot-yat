using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Classes.Managers;

public partial class CommandManager : Node
{
    /// <summary>
    /// Signal emitted when a command execution has started.
    /// </summary>
    /// <param name="command">The command that was started.</param>
    /// <param name="args">The arguments passed to the command.</param>
    [Signal]
    public delegate void CommandStartedEventHandler(string command, string[] args);

    /// <summary>
    /// Signal emitted when a command has been executed.
    /// </summary>
    /// <param name="command">The command that was executed.</param>
    /// <param name="args">The arguments passed to the command.</param>
    /// <param name="result">The result of the command execution.</param>
    [Signal]
    public delegate void CommandFinishedEventHandler(string command, string[] args, ECommandResult result);

    public CancellationTokenSource Cts { get; set; } = new();

#nullable disable
    private YAT _yat;
#nullable restore

    public override void _Ready()
    {
        _yat = GetNode<YAT>("/root/YAT");
    }

    public async Task<bool> RunAsync(string[] args, BaseTerminal terminal)
    {
        if (args.Length == 0)
        {
            return false;
        }

        Cts = new();
        Dictionary<StringName, object?> convertedArgs = [];
        Dictionary<StringName, object?> convertedOpts = [];

        Pipeline? pipeline = CreatePipeline(
            args,
            terminal,
            ref convertedArgs,
            ref convertedOpts
        );
        if (pipeline is null)
        {
            terminal.Output.Error(Messages.UnknownCommand(args[0]));
            return false;
        }

        CommandData data = new(
            _yat,
            terminal,
            null,
            args,
            convertedArgs!,
            convertedOpts!,
            Cts.Token
        );

        _ = CallDeferredThreadGroup(
            "emit_signal",
            SignalName.CommandStarted,
            args[0],
            args
        );

        CommandResult result = await pipeline.ExecuteAsync(data);
        Cts.Dispose();

        _ = CallDeferredThreadGroup(
            "emit_signal",
            SignalName.CommandFinished,
            args[0],
            args,
            (ushort)result.Result
        );

        terminal.LastCommandResult = result.Result;

        PrintCommandResult(result, terminal);

        return result.Result is ECommandResult.Success or ECommandResult.Ok;
    }

    private static Pipeline? CreatePipeline(
        string[] args,
        BaseTerminal terminal,
        ref Dictionary<StringName, object?> cA,
        ref Dictionary<StringName, object?> cO
    )
    {
        Pipeline pipeline = new();
        List<string> commandBuffer = [];

        foreach (string arg in args)
        {
            if (arg == "|")
            {
                if (commandBuffer.Count > 0)
                {
                    if (!AddCommandToPipeline(
                        [.. commandBuffer],
                        pipeline,
                        terminal,
                        ref cA,
                        ref cO
                    ))
                    {
                        return null;
                    }
                    commandBuffer.Clear();
                }
            }
            else
            {
                commandBuffer.Add(arg);
            }
        }

        return commandBuffer.Count > 0
            && !AddCommandToPipeline(
                [.. commandBuffer],
                pipeline,
                terminal,
                ref cA,
                ref cO
            ) ? null : pipeline;
    }

    private static bool AddCommandToPipeline(
        string[] args,
        Pipeline pipeline,
        BaseTerminal terminal,
        ref Dictionary<StringName, object?> cA,
        ref Dictionary<StringName, object?> cO
    )
    {
        string commandName = args[0];
        string[] commandArgs = args.Length > 1 ? args[1..] : [];

        if (!RegisteredCommands.Registered.TryGetValue(commandName, out Type? commandType))
        {
            terminal.Output.Error(Messages.UnknownCommand(commandName));
            return false;
        }

        ICommand command = (Activator.CreateInstance(commandType) as ICommand)!;

        if (command.GetAttribute<NoValidateAttribute>() is null)
        {
            if (!terminal.CommandValidator.ValidatePassedData<ArgumentAttribute>(
                command, args[1..], out cA
            ))
            {
                return false;
            }

            if (command.GetAttributes<OptionAttribute>() is not null
                && !terminal.CommandValidator.ValidatePassedData<OptionAttribute>(
                    command, args[1..], out cO
                ))
            {
                return false;
            }
        }
        command.Arguments = commandArgs;
        pipeline.AddCommand(command);

        return true;
    }

    private static void PrintCommandResult(CommandResult result, BaseTerminal terminal)
    {
        if (string.IsNullOrEmpty(result.Message))
        {
            return;
        }

        if (result.Result == ECommandResult.Success)
        {
            terminal.Output.Success(result.Message);
            return;
        }

        if (result.Result == ECommandResult.Ok)
        {
            terminal.Output.Print(result.Message);
            return;
        }

        terminal.Output.Error(result.Message);
    }
}
