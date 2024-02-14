using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("pause", "Toggles the game pause state.", "[b]Usage[/b]: pause")]
public sealed class Pause : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		data.Yat.GetTree().Paused = !data.Yat.GetTree().Paused;

		return ICommand.Success();
	}
}
