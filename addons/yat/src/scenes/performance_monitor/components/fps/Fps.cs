using Godot;
using YAT.Interfaces;

namespace YAT.Scenes.PerformanceMonitor
{
	public partial class Fps : PanelContainer, IPerformanceMonitorComponent
	{
		public bool UseColors { get; set; }

		private RichTextLabel _label;
		private YAT _yat;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");
			_label = GetNode<RichTextLabel>("RichTextLabel");
		}

		public void Update()
		{
			var fps = Performance.GetMonitor(Performance.Monitor.TimeFps);
			var opts = _yat.Options;

			_label.Clear();
			if (UseColors) _label.PushColor(fps < 30 ? opts.ErrorColor : opts.SuccessColor);
			_label.AppendText($"{fps} FPS");
			if (UseColors) _label.Pop();
		}
	}
}
