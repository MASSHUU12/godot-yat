using System.Globalization;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("lcr", "Shows the result of the last command.")]
[Option("-l", "bool", "Also displays name of the result.")]
public sealed class Lcr : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        ECommandResult result = data.Terminal.LastCommandResult;

        return ICommand.Ok(
            [result.ToString()],
            (bool)data.Options["-l"]
                ? $"{result} ({(int)result})"
                : ((int)result).ToStringInvariant()
        );
    }
}
