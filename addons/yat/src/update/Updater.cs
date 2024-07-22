using Godot;
using YAT.Classes;
using YAT.Helpers;
using YAT.Types;

namespace YAT.Update;

public static class Updater
{
    private static SemanticVersion? _currentVersion;

    public static bool UpdateToVersion(ReleaseTagInfo release)
    {
        // EditorInterface.Singleton.PopupDialogCentered();

        return true;
    }

    public static SemanticVersion? GetCurrentVersion()
    {
        if (_currentVersion is not null)
        {
            return _currentVersion;
        }

        // Read the information from the project settings
        // because addons can be installed in various locations,
        // so it cannot be assumed that YAT will always be in the default location.
        string yatPath = ProjectSettings.GetSetting("autoload/YAT").AsString();
        string pluginConfigPath = yatPath[1..].Remove(yatPath.Length - 13) + "plugin.cfg";

        ConfigFile config = new();
        if (config.Load(pluginConfigPath) != Error.Ok)
        {
            return null;
        }

        string version = config.GetValue("plugin", "version").AsString();

        _currentVersion = SemanticVersion.TryParse(version, out SemanticVersion? v) ? v : null;
        return _currentVersion;
    }

    public static bool IsUpdateAvailable()
    {
        (bool isSuccess, ReleaseTagInfo? info) = Release.GetLatestVersion();
        SemanticVersion? currentVersion = GetCurrentVersion();

        return isSuccess
            && info is not null
            && currentVersion is not null
            && info.Version > currentVersion;
    }
}
