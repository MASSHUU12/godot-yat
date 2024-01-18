using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("restart", "Restarts the level.", "[b]Usage[/b]: restart", "reboot")]
	public partial class Restart : ICommand
	{
		public CommandResult Execute(CommandData args)
		{
			args.Terminal.Print($"Restarting {args.Yat.GetTree().CurrentScene.Name}...");
			args.Yat.GetTree().ReloadCurrentScene();

			return CommandResult.Success;
		}
	}
}
