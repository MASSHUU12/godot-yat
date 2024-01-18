using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("pause", "Toggles the game pause state.", "[b]Usage[/b]: pause")]
	public partial class Pause : ICommand
	{
		public CommandResult Execute(CommandData data)
		{
			data.Yat.GetTree().Paused = !data.Yat.GetTree().Paused;

			return CommandResult.Success;
		}
	}
}
