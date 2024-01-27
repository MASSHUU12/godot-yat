using Godot;
using YAT.Interfaces;

namespace YAT.Scenes.Monitor;

public partial class Fps : PanelContainer, IMonitorComponent
{
	public bool UseColors { get; set; }

	private RichTextLabel _label;
	private RichTextLabel _times;
	private YAT _yat;

	public override void _Ready()
	{
		_yat = GetNode<YAT>("/root/YAT");
		_label = GetNode<RichTextLabel>("./VBoxContainer/RichTextLabel");
		_times = GetNode<RichTextLabel>("./VBoxContainer/Times");
	}

	public void Update()
	{
		var fps = Performance.GetMonitor(Performance.Monitor.TimeFps);
		var process = Performance.GetMonitor(Performance.Monitor.TimeProcess) * 1000;
		var physics = Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess) * 1000;
		var opts = _yat.OptionsManager.Options;

		_label.Clear();
		if (UseColors) _label.PushColor(fps < 30 ? opts.ErrorColor : opts.SuccessColor);
		_label.AppendText($"{fps} FPS");
		if (UseColors) _label.Pop();

		_times.Clear();
		_times.AppendText("Process: ");
		if (UseColors && fps < 30) _times.PushColor(opts.ErrorColor);
		_times.AppendText($"{process:0.00} ms\n");
		if (UseColors && fps < 30) _times.Pop();
		_times.AppendText("Physics: ");
		if (UseColors && fps < 30) _times.PushColor(opts.ErrorColor);
		_times.AppendText($"{physics:0.000} ms");
		if (UseColors && fps < 30) _times.Pop();

	}
}
