using Godot;
using YAT.Attributes;
using YAT.Interfaces;

namespace YAT.Scenes;

[Title("Engine")]
public partial class EngineInfoItem : PanelContainer, IDebugScreenItem
{
#nullable disable
    private Label _label;
    private string _engineInfo;
#nullable restore

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
