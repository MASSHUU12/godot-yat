using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

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

		public bool Locked { get; private set; }
		public CancellationTokenSource Cts { get; set; }

		private YAT _yat;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("..");
		}

		/// <summary>
		/// Executes the specified command with the given arguments.
		/// </summary>
		/// <param name="args">The arguments to pass to the command.</param>
		public void Run(string[] args)
		{
			if (args.Length == 0) return;

			string commandName = args[0];

			if (!_yat.Commands.ContainsKey(commandName))
			{
				LogHelper.UnknownCommand(commandName);
				return;
			}

			ICommand command = _yat.Commands[commandName];
			Dictionary<string, object> convertedArgs = null;
			Dictionary<string, object> convertedOpts = null;

			if (command.GetAttribute<NoValidateAttribute>() is null)
			{
				if (!CommandHelper.ValidatePassedData<ArgumentAttribute>(
					command, args[1..], out convertedArgs
				)) return;

				if (command.GetAttributes<OptionAttribute>() is not null)
				{
					if (!CommandHelper.ValidatePassedData<OptionAttribute>(
						command, args[1..], out convertedOpts
					)) return;
				}
			}

			var concatenated = ConcatenatePassedData(convertedArgs, convertedOpts);

			EmitSignal(SignalName.CommandStarted, commandName, args);

			if (AttributeHelper.GetAttribute<ThreadedAttribute>(
				_yat.Commands[commandName]
			) is not null)
			{
				ExecuteThreadedCommand(args, concatenated);
				return;
			}

			ExecuteCommand(args, concatenated);
		}

		/// <summary>
		/// Executes the given CLI command.
		/// </summary>
		/// <param name="input">The input arguments for the command.</param>
		private void ExecuteCommand(string[] input, Dictionary<string, object> cArgs)
		{
			string commandName = input[0];
			var command = _yat.Commands[commandName];
			CommandArguments args = new(_yat, _yat.Terminal, command, commandName, input, cArgs, Cts?.Token);

			Locked = true;
			var result = command.Execute(args);
			Locked = false;

			EmitSignal(SignalName.CommandFinished, commandName, input, (ushort)result);
		}

		/// <summary>
		/// Executes a command in a separate thread,
		/// allowing the terminal to remain responsive.
		/// </summary>
		/// <param name="input">The command and its arguments.</param>
		private async void ExecuteThreadedCommand(string[] input, Dictionary<string, object> cArgs)
		{
			Cts = new();

			Task task = new(() =>
			{
				string commandName = input[0];
				var command = _yat.Commands[commandName];
				CommandArguments args = new(_yat, _yat.Terminal, command, commandName, input, cArgs, Cts.Token);

				Locked = true;
				var result = command.Execute(args);
				Locked = false;

				CallDeferredThreadGroup(
					"emit_signal", SignalName.CommandFinished, commandName, input, (ushort)result
				);
			}, Cts.Token);

			task.Start();

			await ToSignal(this, SignalName.CommandFinished);

			_yat.Terminal.Print("Command execution finished.", PrintType.Success);
		}

		/// <summary>
		/// Concatenates two dictionaries and returns the result.
		/// </summary>
		/// <param name="args">The first dictionary to concatenate.</param>
		/// <param name="opts">The second dictionary to concatenate.</param>
		/// <returns>The concatenated dictionary.</returns>
		private static Dictionary<string, object> ConcatenatePassedData(Dictionary<string, object> args, Dictionary<string, object> opts)
		{
			Dictionary<string, object> result = new();

			if (args is not null) result = result.Concat(args).ToDictionary(x => x.Key, x => x.Value);
			if (opts is not null) result = result.Concat(opts).ToDictionary(x => x.Key, x => x.Value);

			return result;
		}
	}
}
