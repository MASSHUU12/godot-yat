using System.Globalization;
using Godot;
using YAT.Attributes;

namespace YAT.Debug;

[Title("Engine")]
public partial class EngineInfoItem : DebugScreenItem
{
#nullable disable
    private RichTextLabel _label;
#nullable restore

    public override void _Ready()
    {
        base._Ready();

        LayoutDirection = LayoutDirectionEnum.Rtl;

        _label = CreateLabel();
        _label.Text = string.Format(
            CultureInfo.InvariantCulture,
            "Godot {0} ({1} template)",
            Engine.GetVersionInfo()["string"].AsString(),
            OS.IsDebugBuild() ? "Debug" : "Release"
        );
        VContainer.AddChild(_label);
    }
}
