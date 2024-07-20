using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("cu")]
[Description("Check if YAT update is available.")]
public sealed class CheckUpdate : ICommand
{
    public CommandResult Execute(CommandData _)
    {
        (bool isSuccess, ReleaseTagInfo? info) = Release.GetLatestVersion();

        return isSuccess
            ? ICommand.Ok(info!.Version.ToString())
            : ICommand.Failure("ERR");
    }
}
