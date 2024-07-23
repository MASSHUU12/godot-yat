using YAT.Attributes;
using YAT.Classes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;
using YAT.Update;

namespace YAT.Commands;

[Command("cu")]
[Description("Check if YAT update is available.")]
public sealed class CheckUpdate : ICommand
{
    public CommandResult Execute(CommandData _)
    {
        (bool isSuccess, ReleaseTagInfo? info) = Release.GetLatestVersion();
        SemanticVersion? currentVersion = Updater.GetCurrentVersion();

        if (!isSuccess || info is null)
        {
            return ICommand.Failure(
                "YAT was unable to download information about new releases."
            );
        }

        if (currentVersion is null)
        {
            return ICommand.Failure("YAT version information is unavailable.");
        }

        if (info.Version == currentVersion)
        {
            return ICommand.Success($"YAT {currentVersion} is up-to-date.");
        }

        if (info.Version > currentVersion)
        {
            return ICommand.Success(
                $"A new version of YAT {info.Version} is available.\n"
                + "Navigate to the editor to update YAT."
            );
        }

        return ICommand.Failure("Something went wrong.");
    }
}
