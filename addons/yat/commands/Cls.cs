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
		public void Execute(YAT yat, params string[] args)
		{
			yat.Terminal.Clear();
		}
	}

}
