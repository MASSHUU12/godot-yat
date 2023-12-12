#if TOOLS
using Godot;

[Tool]
public partial class Plugin : EditorPlugin
{
	public override void _EnterTree()
	{
		AddAutoloadSingleton("YAT", "res://addons/yat/src/YAT.tscn");
		GD.Print($"YAT {GetPluginVersion()} loaded!");
		GD.PrintRich("Up to date information about YAT can be found at [url=https://github.com/MASSHUU12/godot-yat/tree/main]godot-yat[/url] on GitHub.");
	}

	public override void _ExitTree()
	{
		RemoveAutoloadSingleton("YAT");
		GD.Print($"YAT {GetPluginVersion()} unloaded!");
	}
}
#endif
