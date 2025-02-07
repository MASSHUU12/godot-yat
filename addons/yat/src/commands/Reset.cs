using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command(
    "reset",
    "Resets the terminal to its default position and/or size."
)]
[Argument("action", "all|position|size", "The action to perform.")]
public sealed class Reset : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        switch ((string)data.Arguments["action"])
        {
            case "size":
                _ = data.Terminal.EmitSignal(nameof(data.Terminal.SizeResetRequested));
                break;
            case "position":
                _ = data.Terminal.EmitSignal(nameof(data.Terminal.PositionResetRequested));
                break;
            case "all":
                _ = data.Terminal.EmitSignal(nameof(data.Terminal.SizeResetRequested));
                _ = data.Terminal.EmitSignal(nameof(data.Terminal.PositionResetRequested));
                break;
            default:
                break;
        }

        return ICommand.Success();
    }
}
