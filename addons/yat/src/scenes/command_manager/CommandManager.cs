using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Scenes.CommandManager
{
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
		public Dictionary<string, ICommand> Commands { get; private set; } = new();

		private YAT _yat;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("..");
		}

		/// <summary>
		/// Adds a CLICommand to the list of available commands.
		/// </summary>
		/// <param name="command">The CLICommand to add.</param>
		public void AddCommand(ICommand command)
		{
			if (AttributeHelper.GetAttribute<CommandAttribute>(command)
				is not CommandAttribute attribute)
			{
				_yat.CurrentTerminal.Output.Error(
					Messages.MissingAttribute("CommandAttribute", command.GetType().Name)
				);
				return;
			}

			Commands[attribute.Name] = command;
			foreach (string alias in attribute.Aliases) Commands[alias] = command;
		}

		public void AddCommand(params ICommand[] commands)
		{
			foreach (var command in commands) AddCommand(command);
		}

		public void Run(string[] args, BaseTerminal.BaseTerminal terminal)
		{
			if (args.Length == 0) return;

			string commandName = args[0];

			if (!Commands.ContainsKey(commandName))
			{
				_yat.CurrentTerminal.Output.Error(Messages.UnknownCommand(commandName));
				return;
			}

			ICommand command = Commands[commandName];
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

			if (AttributeHelper.GetAttribute<ThreadedAttribute>(
				Commands[commandName]
			) is not null)
			{
				ExecuteThreadedCommand(data);
				return;
			}

			ExecuteCommand(data);
		}

		private void ExecuteCommand(CommandData data)
		{
			string commandName = data.RawData[0];
			var command = Commands[commandName];

			data.Terminal.Locked = true;
			var result = command.Execute(data);
			data.Terminal.Locked = false;

			CallDeferredThreadGroup(
				"emit_signal", SignalName.CommandFinished, commandName, data.RawData, (ushort)result
			);
		}

		private async void ExecuteThreadedCommand(CommandData data)
		{
			new Task(() => ExecuteCommand(data), Cts.Token).Start();
			await ToSignal(this, SignalName.CommandFinished);

			_yat.CurrentTerminal.Output.Success("Command execution finished.");
		}

		private static Dictionary<string, object> ConcatenatePassedData(Dictionary<string, object> args, Dictionary<string, object> opts)
		{
			Dictionary<string, object> result = new();

			if (args is not null) result = result.Concat(args).ToDictionary(x => x.Key, x => x.Value);
			if (opts is not null) result = result.Concat(opts).ToDictionary(x => x.Key, x => x.Value);

			return result;
		}
	}
}
