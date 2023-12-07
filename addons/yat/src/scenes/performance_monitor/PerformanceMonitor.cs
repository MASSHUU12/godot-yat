using Godot;
using YAT.Interfaces;

namespace YAT.Scenes.PerformanceMonitor
{
	public partial class PerformanceMonitor : YatWindow.YatWindow
	{
		private YAT _yat;
		private Timer _timer;
		private VBoxContainer _components;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");
			_timer = GetNode<Timer>("Timer");
			_components = GetNode<VBoxContainer>("%Components");

			Move(WindowPosition.TopRight, 16);

			AddComponent(GD.Load<PackedScene>("res://addons/yat/src/scenes/performance_monitor/components/fps/Fps.tscn").Instantiate<Fps>());
		}

		/// <summary>
		/// Adds a component to the performance monitor.
		/// </summary>
		/// <param name="component">The component to add.</param>
		public void AddComponent(Node component, bool useColors = true)
		{
			if (component is not IPerformanceMonitorComponent)
			{
				_yat.Terminal.Print(
					$"Component {component.Name} does not implement IPerformanceMonitorComponent",
					Terminal.Terminal.PrintType.Error
				);
				return;
			}

			var cmp = component as IPerformanceMonitorComponent;

			_components.AddChild(component);

			cmp.UseColors = useColors;
			_timer.Timeout += cmp.Update;
		}
	}
}
