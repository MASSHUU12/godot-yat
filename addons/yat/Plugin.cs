#if TOOLS
using Godot;
using YAT.Classes;
using YAT.Helpers;
using YAT.Types;
using YAT.Update;

namespace YAT;

[Tool]
public partial class Plugin : EditorPlugin
{
    private string _version = string.Empty;

    public override void _EnterTree()
    {
        _version = GetPluginVersion();

        AddAutoloadSingleton("YAT", GetPluginPath() + "/src/YAT.tscn");
        AddAutoloadSingleton("DebugScreen", GetPluginPath() + "/src/scenes/debug_screen/DebugScreen.tscn");

        GD.Print($"YAT {_version} loaded!");
        GD.PrintRich("Up to date information about YAT can be found at [url=https://github.com/MASSHUU12/godot-yat/tree/main]https://github.com/MASSHUU12/godot-yat/tree/main[/url].");

        if (Engine.IsEditorHint())
        {
            UpdateCheck();
        }
    }

    public override void _ExitTree()
    {
        RemoveAutoloadSingleton("YAT");
        RemoveAutoloadSingleton("DebugScreen");

        GD.Print($"YAT {_version} unloaded!");
    }

    private void OpenUpdater(SemanticVersion currentVersion, ReleaseTagInfo info)
    {
        UpdaterWindow window = GD.Load<PackedScene>(
            GetPluginPath() + "/src/update/UpdaterWindow.tscn"
        ).Instantiate<UpdaterWindow>();

        window.UpdateInfo = info;
        window.CurrentVersion = currentVersion;

        EditorInterface.Singleton.PopupDialogCentered(window);
    }

    private string GetPluginPath()
    {
        return GetScript().As<Script>().ResourcePath.GetBaseDir();
    }

    private void UpdateCheck()
    {
        const bool DISABLE_UPDATE_CHECK = false;

        if (DISABLE_UPDATE_CHECK)
        {
#pragma warning disable CS0162 // Unreachable code detected
            return;
#pragma warning restore CS0162 // Unreachable code detected
        }

        if (Updater.IsUpdateAvailable())
        {
            (bool isSuccess, ReleaseTagInfo? info) = Release.GetLatestVersion();

            if (!isSuccess || info is null)
            {
                GD.PrintErr("Something went wrong when downloading update info for YAT.");
                return;
            }

            GD.PrintRich(
                string.Format(
                    "New YAT version {0} is available, click [url={1}]here[/url] to download.",
                    info.Version,
                    info.ZipballUrl
                )
            );

            OpenUpdater(SemanticVersion.Parse(GetPluginVersion()), info);

            return;
        }

        GD.Print("YAT is up-to-date.");
    }
}
#endif
