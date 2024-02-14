using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command(
	"cls",
	"Clears the console.",
	"[b]Usage[/b]: cls",
	"clear"
)]
public sealed class Cls : ICommand
{
	public ECommandResult Execute(CommandData data)
	{
		data.Terminal.Clear();

		return ECommandResult.Success;
	}
}
