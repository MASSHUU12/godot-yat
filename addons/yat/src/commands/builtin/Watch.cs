using System;
using System.Threading;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Overlay.Components.Terminal;

namespace YAT.Commands
{
	[Command(
		"watch",
		"Runs user-defined (not threaded) commands at regular intervals.",
		"[b]Usage[/b]: watch <command> <interval (in seconds)> [args...]"
	)]
	[Threaded]
	[Arguments("command:string", "interval:float")]
	public partial class Watch : ICommand
	{
		public YAT Yat { get; set; }

		public Watch(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(CancellationToken ct, params string[] args)
		{
			const uint SECONDS_MULTIPLIER = 1000;

			if (!Yat.Commands.TryGetValue(args[1], out ICommand command))
			{
				LogHelper.UnknownCommand(args[1]);
				return CommandResult.Failure;
			}

			if (!float.TryParse(args[2], out float interval))
			{
				LogHelper.InvalidArgument("watch", "interval", "positive number");
				return CommandResult.Failure;
			}

			interval = (float)Math.Clamp(interval, 0.5, 60);

			while (!ct.IsCancellationRequested)
			{
				if (command.Execute(args[2..]) != CommandResult.Success)
				{
					Yat.Terminal.Print(
						$"Error executing command '{args[1]}', exiting watch.", Terminal.PrintType.Error
					);
					return CommandResult.Failure;
				}

				Thread.Sleep((int)(interval * SECONDS_MULTIPLIER));
			}

			return CommandResult.Success;
		}
	}
}
