using YAT.Attributes;

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
		public YAT Yat { get; set; }

		public Cls(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			Yat.Terminal.Clear();

			return CommandResult.Success;
		}
	}
}
