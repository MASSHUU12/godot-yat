using Godot;
using YAT.Attributes;

namespace YAT.Debug;

[Title("OS")]
public partial class OsInfoItem : DebugScreenItem
{
#nullable disable
    private RichTextLabel _label;
#nullable restore

    private readonly string _osName = OS.GetName();
    private readonly string _displayServerName = DisplayServer.GetName();

    public override void _Ready()
    {
        base._Ready();

        LayoutDirection = LayoutDirectionEnum.Rtl;

        _label = CreateLabel();
        _label.Text = $"{_osName} {OS.GetVersion()} {OS.GetLocale()} {(
            _osName == _displayServerName
            ? string.Empty
            : $"({_displayServerName})")}";
        VContainer.AddChild(_label);
    }
}
