using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Scenes.BaseTerminal.Components;

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
	public delegate void CommandFinishedEventHandler(string command, string[] args, CommandResult result);

	public CancellationTokenSource Cts { get; set; } = new();

	private YAT _yat;

	public override void _Ready()
	{
		_yat = GetNode<YAT>("/root/YAT");
	}

	public void Run(string[] args, BaseTerminal terminal)
	{
		if (args.Length == 0) return;

		string commandName = args[0];

		if (!RegisteredCommands.Registered.TryGetValue(commandName, out Type value))
		{
			terminal.Output.Error(Messages.UnknownCommand(commandName));
			return;
		}

		ICommand command = Activator.CreateInstance(value) as ICommand;
		Dictionary<string, object> convertedArgs = null;
		Dictionary<string, object> convertedOpts = null;

		if (command.GetAttribute<NoValidateAttribute>() is null)
		{
			if (!terminal.CommandValidator.ValidatePassedData<ArgumentAttribute>(
				command, args[1..], out convertedArgs
			)) return;

			if (command.GetAttributes<OptionAttribute>() is not null)
			{
				if (!terminal.CommandValidator.ValidatePassedData<OptionAttribute>(
					command, args[1..], out convertedOpts
				)) return;
			}
		}

		EmitSignal(SignalName.CommandStarted, commandName, args);

		Cts = new();
		CommandData data = new(_yat, terminal, command, args, convertedArgs, convertedOpts, Cts.Token);

		if (command.GetAttribute<ThreadedAttribute>() is not null)
		{
			ExecuteThreadedCommand(data);
			return;
		}

		ExecuteCommand(data);
	}

	private void ExecuteCommand(CommandData data)
	{
		string commandName = data.RawData[0];

		data.Terminal.Locked = true;
		var result = data.Command.Execute(data);
		data.Terminal.Locked = false;

		CallDeferredThreadGroup(
			"emit_signal", SignalName.CommandFinished, commandName, data.RawData, (ushort)result
		);
	}

	private async void ExecuteThreadedCommand(CommandData data)
	{
		new Task(() => ExecuteCommand(data), Cts.Token).Start();
		await ToSignal(this, SignalName.CommandFinished);

		data.Terminal.Output.Success("Command execution finished.");
	}

	private static Dictionary<string, object> ConcatenatePassedData(Dictionary<string, object> args, Dictionary<string, object> opts)
	{
		Dictionary<string, object> result = new();

		if (args is not null) result = result.Concat(args).ToDictionary(x => x.Key, x => x.Value);
		if (opts is not null) result = result.Concat(opts).ToDictionary(x => x.Key, x => x.Value);

		return result;
	}
}
