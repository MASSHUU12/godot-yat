using YAT.Attributes;
using YAT.Classes;
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
        (bool isSuccess, SemanticVersion? version) = Release.CheckLatestVersion();

        return isSuccess
            ? ICommand.Ok(version!.ToString())
            : ICommand.Failure("ERR");
    }
}
