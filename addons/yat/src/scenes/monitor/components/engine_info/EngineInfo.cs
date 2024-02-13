using Godot;
using YAT.Interfaces;

namespace YAT.Scenes;

public partial class EngineInfo : PanelContainer, IMonitorComponent
{
	public bool UseColors { get; set; }

	private Label _label;
	private readonly string _engineVersion = Engine.GetVersionInfo()["string"].AsString();
	private readonly bool _isDebug = OS.IsDebugBuild();
	private string _engineInfo;

	public override void _Ready()
	{
		_engineInfo = $"Godot {_engineVersion} ({(_isDebug ? "Debug" : "Release")} template)";

		_label = GetNode<Label>("Label");
		_label.Text = _engineInfo;
	}

	public void Update() { }
}
