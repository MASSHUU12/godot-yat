namespace YAT.Commands
{
	[Command(
	"cls",
	"Clears the console.",
	"[b]Usage[/b]: cls",
	"clear"
)]
	public partial class Cls : ICommand
	{
		public CommandResult Execute(YAT yat, params string[] args)
		{
			yat.Terminal.Clear();

			return CommandResult.Success;
		}
	}

}
