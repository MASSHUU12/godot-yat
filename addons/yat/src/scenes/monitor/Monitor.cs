using Godot;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Scenes.Monitor
{
	public partial class Monitor : YatWindow.YatWindow
	{
		private YAT _yat;
		private Timer _timer;
		private VBoxContainer _components;

		public override void _Ready()
		{
			base._Ready();

			_yat = GetNode<YAT>("/root/YAT");
			_timer = GetNode<Timer>("Timer");
			_components = GetNode<VBoxContainer>("%Components");

			Move(WindowPosition.TopLeft, 16);
		}

		/// <summary>
		/// Adds a component to the performance monitor.
		/// </summary>
		/// <param name="component">The component to add.</param>
		public void AddComponent(Node component, bool useColors = true)
		{
			if (component is not IMonitorComponent)
			{
				Log.Error(Messages.MissingInterface(component.Name, "IMonitorComponent"));
				return;
			}

			var cmp = component as IMonitorComponent;

			_components.AddChild(component);

			cmp.UseColors = useColors;
			_timer.Timeout += cmp.Update;
		}
	}
}
