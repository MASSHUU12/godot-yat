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

		public CommandResult Execute(CommandArguments args)
		{
			ICommand command = args.Yat.Commands.TryGetValue((string)args.ConvertedArgs["command"], out var c) ? c : null;

			if (command is null)
			{
				Log.Error($"Command '{args.ConvertedArgs["command"]}' not found, exiting watch.");
				return CommandResult.InvalidArguments;
			}

			float interval = (float)args.ConvertedArgs["interval"];
			CommandArguments newArgs = args with { Arguments = args.Arguments[2..] };

			while (!args.CancellationToken.Value.IsCancellationRequested)
			{
				if (command.Execute(newArgs) != CommandResult.Success)
				{
					Log.Error($"Error executing command '{args.Arguments[1]}', exiting watch.");
					return CommandResult.Failure;
				}

				Thread.Sleep((int)(interval * SECONDS_MULTIPLIER));
			}

			return CommandResult.Success;
		}
	}
}
