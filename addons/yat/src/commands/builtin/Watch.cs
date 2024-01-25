using System;
using System.Threading;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes;

namespace YAT.Commands
{
	[Command(
		"watch",
		"Runs user-defined (not threaded) commands at regular intervals.",
		"[b]Usage[/b]: watch <command> <interval (in seconds)> [args...]"
	)]
	[Threaded]
	[Argument("command", "string", "The command to run.")]
	[Argument("interval", "float(0.5, 60)", "The interval at which to run the command.")]
	public partial class Watch : ICommand
	{
		private const uint SECONDS_MULTIPLIER = 1000;

		public CommandResult Execute(CommandData data)
		{
			if (!RegisteredCommands.Registered.TryGetValue((string)data.Arguments["command"], out var type))
			{
				data.Terminal.Output.Error(
					$"Command '{data.Arguments["command"]}' not found, exiting watch."
				);
				return CommandResult.InvalidArguments;
			}

			ICommand command = (ICommand)Activator.CreateInstance(type);

			float interval = (float)data.Arguments["interval"] * SECONDS_MULTIPLIER;
			CommandData newArgs = data with { RawData = data.RawData[2..] };

			while (!data.CancellationToken.IsCancellationRequested)
			{
				if (command.Execute(newArgs) != CommandResult.Success)
				{
					data.Terminal.Output.Error(
						$"Error executing command '{data.RawData[1]}', exiting watch."
					);
					return CommandResult.Failure;
				}

				Thread.Sleep((int)interval);
			}

			return CommandResult.Success;
		}
	}
}
