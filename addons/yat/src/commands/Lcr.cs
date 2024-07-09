using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("lcr", "Shows the result of the last command.")]
[Option("-l", "bool", "Also displays name of the result.")]
public sealed class Lcr : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        return ICommand.Ok(
            (bool)data.Options["-l"]
                ? $"{data.Terminal.LastCommandResult} ({(int)data.Terminal.LastCommandResult})"
                : ((int)data.Terminal.LastCommandResult).ToString()
        );
    }
}
