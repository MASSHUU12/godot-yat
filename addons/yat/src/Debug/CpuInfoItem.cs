using Godot;
using YAT.Attributes;

namespace YAT.Debug;

[Title("CPU")]
public partial class CpuInfoItem : DebugScreenItem
{
#nullable disable
    private RichTextLabel _label;
#nullable restore

    private readonly string _arch = Engine.GetArchitectureName();
    private readonly string _cpuName = OS.GetProcessorName();
    private readonly int _cpuCount = OS.GetProcessorCount();

    public override void _Ready()
    {
        base._Ready();

        LayoutDirection = LayoutDirectionEnum.Rtl;

        _label = CreateLabel();
        _label.Text = $"{_cpuName} {_arch} ({_cpuCount} cores)";
        VContainer.AddChild(_label);
    }
}
