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

    public bool Run(string[] args, BaseTerminal terminal)
    {
        if (args.Length == 0)
        {
            return false;
        }

        string commandName = args[0];

        if (!RegisteredCommands.Registered.TryGetValue(commandName, out Type? value))
        {
            terminal.Output.Error(Messages.UnknownCommand(commandName));
            return false;
        }

        ICommand command = (Activator.CreateInstance(value) as ICommand)!;
        Dictionary<StringName, object?> convertedArgs = new();
        Dictionary<StringName, object?> convertedOpts = new();

        if (command.GetAttribute<NoValidateAttribute>() is null)
        {
            if (!terminal.CommandValidator.ValidatePassedData<ArgumentAttribute>(
                command, args[1..], out convertedArgs
            ))
            {
                return false;
            }

            if (command.GetAttributes<OptionAttribute>() is not null
                && !terminal.CommandValidator.ValidatePassedData<OptionAttribute>(
                    command, args[1..], out convertedOpts
                ))
            {
                return false;
            }
        }

        _ = CallDeferredThreadGroup(
            "emit_signal",
            SignalName.CommandStarted,
            commandName,
            args
        );

        Cts = new();
        CommandData data = new(_yat, terminal, command, args, convertedArgs!, convertedOpts!, Cts.Token);

        if (command.GetAttribute<ThreadedAttribute>() is not null)
        {
            ExecuteThreadedCommand(data);
            return false;
        }

        ExecuteCommand(data);

        // Prevent creating orphans
        if (command is Node node)
        {
            node.QueueFree();
        }

        return true;
    }

    private void ExecuteCommand(CommandData data)
    {
        string commandName = data.RawData[0];

        data.Terminal.Locked = true;
        CommandResult result = data.Command.Execute(data);
        data.Terminal.Locked = false;

        Cts.Dispose();

        _ = CallDeferredThreadGroup(
            "emit_signal",
            SignalName.CommandFinished,
            commandName,
            data.RawData,
            (ushort)result.Result
        );

        data.Terminal.LastCommandResult = result.Result;

        PrintCommandResult(result, data.Terminal);
    }

    private async void ExecuteThreadedCommand(CommandData data)
    {
        new Task(() => ExecuteCommand(data), Cts.Token).Start();
        _ = await ToSignal(this, SignalName.CommandFinished);

        data.Terminal.Output.Success($"Command {data.Command.GetType().Name} finished.");
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
