using Godot;
using YAT.Interfaces;

namespace YAT.Scenes.Monitor
{
	public partial class SceneObjects : PanelContainer, IMonitorComponent
	{
		public bool UseColors { get; set; }

		private Label _label;

		public override void _Ready()
		{
			_label = GetNode<Label>("Label");
		}

		public void Update()
		{
			var objects = Performance.GetMonitor(Performance.Monitor.ObjectCount);
			var resources = Performance.GetMonitor(Performance.Monitor.ObjectResourceCount);
			var nodes = Performance.GetMonitor(Performance.Monitor.ObjectNodeCount);
			var orphans = Performance.GetMonitor(Performance.Monitor.ObjectOrphanNodeCount);

			_label.Text = $"Objects: {objects}\nResources: {resources}\nNodes: {nodes}\nOrphans: {orphans}";
		}
	}
}
