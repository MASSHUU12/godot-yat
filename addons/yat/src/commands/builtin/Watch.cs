using System.Collections.Generic;
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
		public YAT Yat { get; set; }

		public Watch(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(Dictionary<string, object> cArgs, CancellationToken ct, params string[] args)
		{
			const uint SECONDS_MULTIPLIER = 1000;

			ICommand command = Yat.Commands.TryGetValue((string)cArgs["command"], out var c) ? c : null;

			if (command == null)
			{
				LogHelper.Error($"Command '{cArgs["command"]}' not found, exiting watch.");
				return CommandResult.InvalidArguments;
			}

			float interval = (float)cArgs["interval"];

			while (!ct.IsCancellationRequested)
			{
				if (command.Execute(args[2..]) != CommandResult.Success)
				{
					LogHelper.Error($"Error executing command '{args[1]}', exiting watch.");
					return CommandResult.Failure;
				}

				Thread.Sleep((int)(interval * SECONDS_MULTIPLIER));
			}

			return CommandResult.Success;
		}
	}
}
