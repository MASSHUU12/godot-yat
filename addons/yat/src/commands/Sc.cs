using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("sc", "Makes a screenshot.")]
[Usage("sc")]
[Option("-cp", "bool", "Copy the screenshot to the clipboard.")]
[Option(
	"-path",
	"string",
	"Save the screenshot to the specified path. If not specified, the screenshot will be saved to the current directory."
)]
public sealed class Sc : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		return ICommand.Success();
	}
}
