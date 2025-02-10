using System.Globalization;
using Godot;
using YAT.Attributes;

namespace YAT.Debug;

[Title("GPU")]
public partial class GpuInfoItem : DebugScreenItem
{
    private readonly RenderingDevice _device = RenderingServer.GetRenderingDevice();

#nullable disable
    private RichTextLabel _label;
    private string _vendor;
    private string _deviceName;
#nullable restore

    public override void _Ready()
    {
        base._Ready();

        LayoutDirection = LayoutDirectionEnum.Rtl;

        _deviceName = _device.GetDeviceName();
        _vendor = _device.GetDeviceVendorName();

        _label = CreateLabel();
        VContainer.AddChild(_label);
    }

    public override void Update()
    {
        Vector2I monitorSize = DisplayServer.ScreenGetSize(
            DisplayServer.WindowGetCurrentScreen()
        );

        _label.Text = string.Format(
            CultureInfo.InvariantCulture,
            "{0} {1} {2}x{3} ({4}x{5})",
            _deviceName,
            _vendor,
            monitorSize.X,
            monitorSize.Y,
            _device.ScreenGetWidth(),
            _device.ScreenGetHeight()
        );
    }
}
