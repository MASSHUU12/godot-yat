using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("echo", "Displays the given text.", "[b]Usage[/b]: echo [i]text[/i]")]
[Argument("message", "string", "The text to display.")]
public sealed class Echo : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var text = string.Join(" ", data.RawData[1..^0]);
		data.Terminal.Print(text);

		return ICommand.Success();
	}
}
