using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("pause", "Toggles the game pause state.", "[b]Usage[/b]: pause")]
	public partial class Pause : ICommand
	{
		public CommandResult Execute(CommandArguments args)
		{
			args.Yat.GetTree().Paused = !args.Yat.GetTree().Paused;

			return CommandResult.Success;
		}
	}
}
