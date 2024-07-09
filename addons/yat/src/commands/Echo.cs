using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("echo")]
[Description("Displays the specified text.")]
[Argument("test", "enum(A:1,B:2)")]
[Argument("message", "string", "The text to display.")]
public sealed class Echo : ICommand
{
    public CommandResult Execute(CommandData data) =>
        ICommand.Ok(string.Join(' ', data.RawData[1..]));
}
