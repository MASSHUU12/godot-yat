using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("echo", "Displays the given text.", "[b]Usage[/b]: echo [i]text[/i]")]
[Argument("message", "string", "The text to display.")]
public sealed class Echo : ICommand
{
	public ECommandResult Execute(CommandData data)
	{
		var text = string.Join(" ", data.RawData[1..^0]);
		data.Terminal.Print(text);

		return ECommandResult.Success;
	}
}
