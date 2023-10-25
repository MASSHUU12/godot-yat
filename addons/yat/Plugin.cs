#if TOOLS
using Godot;

[Tool]
public partial class Plugin : EditorPlugin
{
	public override void _EnterTree()
	{
		AddAutoloadSingleton("YAT", "res://addons/yat/YAT.tscn");
		GD.Print("YAT loaded!");
	}

	public override void _ExitTree()
	{
		RemoveAutoloadSingleton("YAT");
		GD.Print("YAT unloaded!");
	}
}
#endif
