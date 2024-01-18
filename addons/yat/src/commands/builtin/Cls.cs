using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command(
		"cls",
		"Clears the console.",
		"[b]Usage[/b]: cls",
		"clear"
	)]
	public sealed class Cls : ICommand
	{
		public CommandResult Execute(CommandData data)
		{
			data.Terminal.Clear();

			return CommandResult.Success;
		}
	}
}
