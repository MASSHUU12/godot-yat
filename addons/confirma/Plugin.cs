#if TOOLS
using Confirma.Helpers;
using Godot;

namespace Confirma;

[Tool]
public partial class Plugin : EditorPlugin
{
#nullable disable
    private Control _testBottomPanel;
#nullable restore

    public override void _EnterTree()
    {
        _testBottomPanel = GD.Load<PackedScene>(
            GetPluginPath()
            + "/src/scenes/confirma_bottom_panel/ConfirmaBottomPanel.tscn"
        ).Instantiate<Control>();
        _ = AddControlToBottomPanel(_testBottomPanel, "Confirma");

        AddAutoloadSingleton(
            "Confirma",
            GetPluginPath()
            + "/src/scenes/confirma_autoload/ConfirmaAutoload.tscn"
        );

        SetUpSettings();

        GD.Print("Confirma is ready!");
    }

    public override void _ExitTree()
    {
        RemoveAutoloadSingleton("Confirma");
        RemoveControlFromBottomPanel(_testBottomPanel);

        _testBottomPanel.Free();

        GD.Print("Confirma is disabled!");
    }

    private string GetPluginPath()
    {
        return GetScript().As<Script>().ResourcePath.GetBaseDir();
    }

    public static string GetPluginLocation()
    {
        // Read the information from the project settings
        // because addons can be installed in various locations,
        // so it cannot be assumed that it will always be in the default location.
        string p = ProjectSettings.GetSetting("autoload/Confirma").AsString();
        return p[1..][..(p.Length - 51)];
    }

    public static void SetUpSettings()
    {
        // Note: When changing values here,
        // remember to change it also in TestsProps.cs if needed.

        _ = Settings.CreateSetting(
            "confirma/config/gdscript_tests_folder",
            "res://gdtests/",
            "res://gdtests/",
            true,
            true
        );

        _ = Settings.CreateSetting(
            "confirma/config/output_path",
            "./test_results.json",
            "./test_results.json",
            true,
            true
        );
    }
}
#endif
