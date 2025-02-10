using Godot;
using Godot.Collections;
using YAT.Attributes;
using YAT.Helpers;

namespace YAT.Debug;

[Title("Memory")]
public partial class MemoryInfoItem : DebugScreenItem
{
#nullable disable
    private RichTextLabel _label;
#nullable restore

    public override void _Ready()
    {
        base._Ready();

        _label = CreateLabel(new(0.552941f, 0.647059f, 0.952941f, 1));
        VContainer.AddChild(_label);
    }

    public override void Update()
    {
        Dictionary mem = Godot.OS.GetMemoryInfo();
        long physical = mem["physical"].AsInt64();
        long free = mem["free"].AsInt64();
        long stack = mem["stack"].AsInt64();
        double vram = Performance.GetMonitor(Performance.Monitor.RenderVideoMemUsed);

        float freePercent = free / physical * 100f;

        _label.Clear();
        _label.AppendText(
            $"RAM: {(freePercent < 15
                ? ColorText(
                    Numeric.SizeToString(free, 3),
                    Yat.PreferencesManager.Preferences.ErrorColor
                )
                : Numeric.SizeToString(free, 3)
            )}/{Numeric.SizeToString(physical, 3)}\n"
            + $"Stack: {Numeric.SizeToString(stack, 1)}\n"
            + $"VRAM: {Numeric.SizeToString((long)vram, 2)}"
        );
    }
}
