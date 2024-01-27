using Godot;
using YAT.Interfaces;

namespace YAT.Scenes.Monitor;

public partial class CpuInfo : PanelContainer, IMonitorComponent
{
	public bool UseColors { get; set; }

	private readonly string _arch = Engine.GetArchitectureName();
	private readonly string _cpuName = OS.GetProcessorName();
	private readonly int _cpuCount = OS.GetProcessorCount();
	private string _cpuInfo;

	private Label _label;

	public override void _Ready()
	{
		_label = GetNode<Label>("Label");

		_cpuInfo = $"{_cpuName} {_arch} ({_cpuCount} cores)";

		_label.Text = _cpuInfo;
	}

	public void Update() { }
}
