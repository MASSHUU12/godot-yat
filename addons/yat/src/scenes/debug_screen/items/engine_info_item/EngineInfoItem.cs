using Godot;
using YAT.Interfaces;

namespace YAT.Scenes;

public partial class EngineInfoItem : PanelContainer, IDebugScreenItem
{
	public string Title { get; set; } = "Engine";

	private Label _label;
	private string _engineInfo;

	private readonly bool _isDebug = OS.IsDebugBuild();
	private readonly string _engineVersion = Engine.GetVersionInfo()["string"].AsString();

	public override void _Ready()
	{
		_engineInfo = $"Godot {_engineVersion} ({(_isDebug ? "Debug" : "Release")} template)";

		_label = GetNode<Label>("Label");
		_label.Text = _engineInfo;
	}

	public void Update() { }
}
