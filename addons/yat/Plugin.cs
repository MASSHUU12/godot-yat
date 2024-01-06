#if TOOLS
using Godot;

[Tool]
public partial class Plugin : EditorPlugin
{
	private string _version;
	private const string _name = "YAT";

	private Control _editorTerminal;

	public override void _Notification(int what)
	{
		if (what == NotificationParented) _version = GetPluginVersion();
	}

	public override void _EnterTree()
	{
		_editorTerminal = GD.Load<PackedScene>("res://addons/yat/src/scenes/editor_terminal/EditorTerminal.tscn").Instantiate<Control>();

		AddAutoloadSingleton(_name, "res://addons/yat/src/YAT.tscn");
		AddControlToBottomPanel(
			_editorTerminal,
			"Terminal"
		);

		GD.Print($"{_name} {_version} loaded!");
		GD.PrintRich("Up to date information about YAT can be found at [url=https://github.com/MASSHUU12/godot-yat/tree/main]https://github.com/MASSHUU12/godot-yat/tree/main[/url].");
	}

	public override void _ExitTree()
	{
		RemoveAutoloadSingleton(_name);
		// RemoveControlFromBottomPanel(_editorTerminal);
		GD.Print($"{_name} {_version} unloaded!");
	}
}
#endif
