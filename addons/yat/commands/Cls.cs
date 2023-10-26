namespace YAT
{
	[Command(
	"cls",
	"Clears the console.",
	"[b]Usage[/b]: cls",
	"clear"
)]
	public partial class Cls : IYatCommand
	{
		public void Execute(YAT yat, params string[] args)
		{
			yat.Terminal.Clear();
		}
	}

}
