using Godot;
using YAT.Interfaces;

namespace YAT.Scenes;

public partial class GpuInfoItem : PanelContainer, IDebugScreenItem
{
	public string Title { get; set; } = "GPU";

	private readonly RenderingDevice _device = RenderingServer.GetRenderingDevice();

	private Label _label;
	private string _vendor;
	private string _deviceName;

	public override void _Ready()
	{
		_label = GetNode<Label>("Label");

		_deviceName = _device.GetDeviceName();
		_vendor = _device.GetDeviceVendorName();
	}

	public void Update()
	{
		var monitorSize = DisplayServer.ScreenGetSize(
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
