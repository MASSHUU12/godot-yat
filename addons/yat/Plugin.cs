#if TOOLS
using Godot;

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

        GD.Print($"{_version} loaded!");
        GD.PrintRich("Up to date information about YAT can be found at [url=https://github.com/MASSHUU12/godot-yat/tree/main]https://github.com/MASSHUU12/godot-yat/tree/main[/url].");
    }

    public override void _ExitTree()
    {
        RemoveAutoloadSingleton("YAT");
        RemoveAutoloadSingleton("DebugScreen");

        GD.Print($"YAT {_version} unloaded!");
    }

    private string GetPluginPath()
    {
        return GetScript().As<Script>().ResourcePath.GetBaseDir();
    }
}
#endif
