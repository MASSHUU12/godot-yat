using Godot;
using YAT.Attributes;
using YAT.Interfaces;

namespace YAT.Scenes;

[Title("CPU")]
public partial class CpuInfoItem : PanelContainer, IDebugScreenItem
{
#nullable disable
    private Label _label;
    private string _cpuInfo;
#nullable restore

    private readonly string _arch = Engine.GetArchitectureName();
    private readonly string _cpuName = OS.GetProcessorName();
    private readonly int _cpuCount = OS.GetProcessorCount();

    public override void _Ready()
    {
        _cpuInfo = $"{_cpuName} {_arch} ({_cpuCount} cores)";

        _label = GetNode<Label>("Label");
        _label.Text = _cpuInfo;
    }

    public void Update() { }
}
