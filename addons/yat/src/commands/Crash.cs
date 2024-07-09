using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("crash", "Crashes the game.")]
[Option("-message", "string", "The message to when crashing.", ":c")]
public sealed class Crash : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        var message = (string)data.Options["-message"];

        OS.Crash(message);

        return ICommand.Success();
    }
}
