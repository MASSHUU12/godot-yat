using Godot;
using YAT.Types;

namespace YAT.Classes;

public static class Updater
{
    private static SemanticVersion? _currentVersion = null;

    public static bool UpdateToVersion(ReleaseTagInfo release)
    {
        return true;
    }

    public static SemanticVersion? GetCurrentVersion()
    {
        if (_currentVersion is not null) return _currentVersion;

        // Read the information from the project settings
        // because addons can be installed in various locations,
        // so it cannot be assumed that YAT will always be in the default location.
        string yatPath = ProjectSettings.GetSetting("autoload/YAT").AsString();
        string pluginConfigPath = yatPath[1..].Remove(yatPath.Length - 13) + "plugin.cfg";

        ConfigFile config = new();
        if (config.Load(pluginConfigPath) != Error.Ok) return null;

        string version = config.GetValue("plugin", "version").AsString();

        _currentVersion = SemanticVersion.TryParse(version, out var v) ? v : null;
        return _currentVersion;
    }
}
