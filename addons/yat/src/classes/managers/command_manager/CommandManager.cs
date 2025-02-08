using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Godot;
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

    public bool Run(string[] args, BaseTerminal terminal)
    {
        if (args.Length == 0)
        {
            return false;
        }

        Cts = new();

        Pipeline? pipeline = CreatePipeline(args, terminal);
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
            [],
            [],
            Cts.Token
        );

        data.Terminal.Locked = true;
        CommandResult result = pipeline.Execute(data);
        data.Terminal.Locked = false;

        Cts.Dispose();

        PrintCommandResult(result, terminal);

        return result.Result is ECommandResult.Success or ECommandResult.Ok;
    }

    private static Pipeline? CreatePipeline(
        string[] args,
        BaseTerminal terminal
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
                        terminal
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
                terminal
            ) ? null : pipeline;
    }

    private static bool AddCommandToPipeline(
        string[] args,
        Pipeline pipeline,
        BaseTerminal terminal
    )
    {
        string commandName = args[0];

        if (!RegisteredCommands.Registered.TryGetValue(commandName, out Type? commandType))
        {
            terminal.Output.Error(Messages.UnknownCommand(commandName));
            return false;
        }

        ICommand command = (Activator.CreateInstance(commandType) as ICommand)!;
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
