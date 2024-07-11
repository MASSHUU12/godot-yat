using Godot;
using YAT.Attributes;
using YAT.Interfaces;

namespace YAT.Scenes;

[Title("GPU")]
public partial class GpuInfoItem : PanelContainer, IDebugScreenItem
{
    private readonly RenderingDevice _device = RenderingServer.GetRenderingDevice();

#nullable disable
    private Label _label;
    private string _vendor;
    private string _deviceName;
#nullable restore

    public override void _Ready()
    {
        _label = GetNode<Label>("Label");

        _deviceName = _device.GetDeviceName();
        _vendor = _device.GetDeviceVendorName();
    }

    public void Update()
    {
        Vector2I monitorSize = DisplayServer.ScreenGetSize(
            DisplayServer.WindowGetCurrentScreen()
        );

        _label.Text = string.Format(
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
