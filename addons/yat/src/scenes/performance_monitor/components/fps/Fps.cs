using Godot;
using YAT.Interfaces;

namespace YAT.Scenes.PerformanceMonitor
{
	public partial class Fps : Control, IPerformanceMonitorComponent
	{
		private Label _label;

		public override void _Ready()
		{
			_label = GetNode<Label>("Label");
		}

		public void Update()
		{
			_label.Text = $"{Performance.GetMonitor(Performance.Monitor.TimeFps)} FPS";
		}
	}
}
