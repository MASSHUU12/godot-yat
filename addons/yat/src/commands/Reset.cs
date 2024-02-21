using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command(
	"reset",
	"Resets the terminal to its default position and/or size.",
	"[b]reset[/b] [i]action[/i]"
)]
[Argument("action", "all|position|size", "The action to perform.")]
public sealed class Reset : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var action = (string)data.Arguments["action"];

		switch (action)
		{
			case "size":
				data.Terminal.EmitSignal(nameof(data.Terminal.SizeResetRequested));
				break;
			case "position":
				data.Terminal.EmitSignal(nameof(data.Terminal.PositionResetRequested));
				break;
			case "all":
				data.Terminal.EmitSignal(nameof(data.Terminal.SizeResetRequested));
				data.Terminal.EmitSignal(nameof(data.Terminal.PositionResetRequested));
				break;
		}

		return ICommand.Success();
	}
}
