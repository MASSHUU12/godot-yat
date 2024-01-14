using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command(
		"reset",
		"Resets the terminal to its default position and/or size.",
		"[b]reset[/b] [i]action[/i]"
	)]
	[Argument("action", "[all, position, size]", "The action to perform.")]
	public sealed class Reset : ICommand
	{
		public CommandResult Execute(CommandArguments args)
		{
			return CommandResult.Success;
		}
	}
}
