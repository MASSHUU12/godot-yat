using Godot;
using System;
using System.Collections.Generic;
using YAT.Attributes;
using YAT.Commands;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Scenes;

public partial class RegisteredCommands : Node
{
	[Signal] public delegate void QuickCommandsChangedEventHandler();

	[Export] public Resources.QuickCommands QuickCommands { get; set; } = new();

	public static Dictionary<string, Type> Registered { get; private set; } = new();

	private YAT _yat;
	private const ushort MAX_QUICK_COMMANDS = 10;
	private const string QUICK_COMMANDS_PATH = "user://yat_qc.tres";

	public override void _Ready()
	{
		_yat = GetNode<YAT>("..");

		RegisterBuiltinCOmmands();
	}

	public enum AddingResult
	{
		Success,
		UnknownCommand,
		MissingAttribute,
		ExistentCommand
	}

	/// <summary>
	/// Adds a command to the command manager.
	/// </summary>
	/// <param name="commandType">The type of the command to add.</param>
	public static AddingResult AddCommand(Type commandType)
	{
		var commandInstance = Activator.CreateInstance(commandType);

		if (commandInstance is not ICommand command)
			return AddingResult.UnknownCommand;

		if (Reflection.GetAttribute<CommandAttribute>(command)
			is not CommandAttribute attribute)
			return AddingResult.MissingAttribute;

		if (Registered.ContainsKey(attribute.Name))
			return AddingResult.ExistentCommand;

		Registered[attribute.Name] = commandType;
		foreach (string alias in attribute.Aliases)
		{
			if (Registered.ContainsKey(alias)) return AddingResult.ExistentCommand;

			Registered[alias] = commandType;
		}

		return AddingResult.Success;
	}

	public static AddingResult[] AddCommand(params Type[] commands)
	{
		AddingResult[] results = new AddingResult[commands.Length];

		for (int i = 0; i < commands.Length; i++)
			results[i] = AddCommand(commands[i]);

		return results;
	}

	public bool AddQuickCommand(string name, string command)
	{
		if (QuickCommands.Commands.ContainsKey(name) ||
			QuickCommands.Commands.Count >= MAX_QUICK_COMMANDS
		) return false;

		QuickCommands.Commands.Add(name, command);

		var status = Storage.SaveResource(QuickCommands, QUICK_COMMANDS_PATH);

		GetQuickCommands();

		EmitSignal(SignalName.QuickCommandsChanged);

		return status;
	}

	public bool RemoveQuickCommand(string name)
	{
		if (!QuickCommands.Commands.ContainsKey(name)) return false;

		QuickCommands.Commands.Remove(name);

		var status = Storage.SaveResource(QuickCommands, QUICK_COMMANDS_PATH);

		GetQuickCommands();

		EmitSignal(SignalName.QuickCommandsChanged);

		return status;
	}

	/// <summary>
	/// Retrieves the quick commands from file and adds them to the list of quick commands.
	/// </summary>
	public bool GetQuickCommands()
	{
		var qc = Storage.LoadResource<Resources.QuickCommands>(QUICK_COMMANDS_PATH);

		if (qc is not null) QuickCommands = qc;

		return qc is not null;
	}

	private void RegisterBuiltinCOmmands()
	{
		AddingResult[] results = AddCommand(new[] {
			typeof(Ls),
			typeof(Ip),
			typeof(Cn),
			typeof(Cs),
			typeof(Cls),
			typeof(Man),
			typeof(Set),
			typeof(Cat),
			typeof(Sys),
			typeof(Quit),
			typeof(Echo),
			typeof(List),
			typeof(View),
			typeof(Ping),
			typeof(Wenv),
			typeof(Load),
			typeof(Pause),
			typeof(Watch),
			typeof(Reset),
			typeof(Cowsay),
			typeof(Restart),
			typeof(History),
			typeof(Whereami),
			typeof(Timescale),
			typeof(TraceRoute),
			typeof(ToggleAudio),
			typeof(QuickCommands),
			typeof(Commands.Preferences),
		});

		for (int i = 0; i < results.Length; i++)
		{
			switch (results[i])
			{
				case AddingResult.UnknownCommand:
					_yat.CurrentTerminal.Output.Error(
						Messages.UnknownCommand(results[i].ToString())
					);
					break;
				case AddingResult.MissingAttribute:
					_yat.CurrentTerminal.Output.Error(
						Messages.MissingAttribute("CommandAttribute", results[i].ToString())
					);
					break;
				case AddingResult.ExistentCommand:
					_yat.CurrentTerminal.Output.Error($"Command {results[i]} already exists.");
					break;
				default:
					break;
			}
		}
	}
}
