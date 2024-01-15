using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command(
		"reset",
		"Resets the terminal to its default position and/or size.",
		"[b]reset[/b] [i]action[/i]"
	)]
	[Argument("action", "[all, position, size]", "The action to perform.")]
	public sealed class Reset : ICommand
	{
		public CommandResult Execute(CommandArguments args)
		{
			var action = (string)args.ConvertedArgs["action"];

			switch (action)
			{
				case "size":
					args.Terminal.EmitSignal(nameof(args.Terminal.SizeResetRequested));
					break;
				case "position":
					args.Terminal.EmitSignal(nameof(args.Terminal.PositionResetRequested));
					break;
				case "all":
					args.Terminal.EmitSignal(nameof(args.Terminal.SizeResetRequested));
					args.Terminal.EmitSignal(nameof(args.Terminal.PositionResetRequested));
					break;
			}

			return CommandResult.Success;
		}
	}
}
