using System.Threading;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;

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
			ICommand command = data.Yat.Commands.TryGetValue((string)data.Arguments["command"], out var c) ? c : null;

			if (command is null)
			{
				Log.Error($"Command '{data.Arguments["command"]}' not found, exiting watch.");
				return CommandResult.InvalidArguments;
			}

			float interval = (float)data.Arguments["interval"];
			CommandData newArgs = data with { RawData = data.RawData[2..] };

			while (!data.CancellationToken.Value.IsCancellationRequested)
			{
				if (command.Execute(newArgs) != CommandResult.Success)
				{
					Log.Error($"Error executing command '{data.RawData[1]}', exiting watch.");
					return CommandResult.Failure;
				}

				Thread.Sleep((int)(interval * SECONDS_MULTIPLIER));
			}

			return CommandResult.Success;
		}
	}
}
