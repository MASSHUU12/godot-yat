using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("pause", "Toggles the game pause state.", "[b]Usage[/b]: pause")]
	public partial class Pause : ICommand
	{
		public YAT Yat { get; set; }

		public Pause(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			Yat.GetTree().Paused = !Yat.GetTree().Paused;

			return CommandResult.Success;
		}
	}
}
