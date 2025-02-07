using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("restart", "Restarts the level.", aliases: "reboot")]
public sealed class Restart : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        data.Terminal.Print($"Restarting {data.Yat.GetTree().CurrentScene.Name}...");
        _ = data.Yat.GetTree().ReloadCurrentScene();

        return ICommand.Success();
    }
}
