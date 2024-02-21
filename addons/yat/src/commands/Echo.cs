using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("echo")]
[Usage("echo [i]text[/i]")]
[Description("Displays the specified text.")]
[Argument("message", "string", "The text to display.")]
public sealed class Echo : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		data.Terminal.Print(data.Arguments["message"]);

		return ICommand.Success();
	}
}
