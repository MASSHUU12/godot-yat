using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("restart", "Restarts the level.", "[b]Usage[/b]: restart", "reboot")]
public sealed class Restart : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		data.Terminal.Print($"Restarting {data.Yat.GetTree().CurrentScene.Name}...");
		data.Yat.GetTree().ReloadCurrentScene();

		return ICommand.Success();
	}
}
