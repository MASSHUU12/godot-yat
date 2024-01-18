using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("restart", "Restarts the level.", "[b]Usage[/b]: restart", "reboot")]
	public partial class Restart : ICommand
	{
		public CommandResult Execute(CommandData data)
		{
			data.Terminal.Print($"Restarting {data.Yat.GetTree().CurrentScene.Name}...");
			data.Yat.GetTree().ReloadCurrentScene();

			return CommandResult.Success;
		}
	}
}
