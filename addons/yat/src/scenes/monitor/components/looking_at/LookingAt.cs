using Godot;
using Godot.Collections;
using YAT.Interfaces;

namespace YAT.Scenes.Monitor
{
	public partial class LookingAt : PanelContainer, IMonitorComponent
	{
		public bool UseColors { get; set; }

		private YAT _yat;
		private Label _label;
		private const uint RAY_LENGTH = 1000;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");
			_label = GetNode<Label>("Label");
		}

		public void Update()
		{
			var prefix = "Looking at: ";
			var result = RayCast();

			if (result is null) return;

			if (result.Count == 0)
			{
				_label.Text = prefix + "Nothing";
				return;
			}

			Vector2 position = result["position"].As<Vector2>();
			Node node = result["collider"].As<Node>();

			if (node is null)
			{
				_label.Text = prefix + "Nothing";
				return;
			}

			_label.Text = prefix + node.Name + " at " + position;
		}

		private Dictionary RayCast()
		{
			var viewport = _yat.GetViewport();
			var camera = viewport.GetCamera3D();
			var mousePos = viewport.GetMousePosition();
			var prefix = "Looking at: ";

			if (camera == null)
			{
				_label.Text = prefix + "No camera";
				return null;
			}

			var origin = camera.ProjectRayOrigin(mousePos);
			var end = origin + camera.ProjectRayNormal(mousePos) * RAY_LENGTH;
			var query = PhysicsRayQueryParameters3D.Create(origin, end);
			query.CollideWithAreas = true;
			query.CollideWithBodies = true;

			return camera.GetWorld3D().DirectSpaceState.IntersectRay(query);
		}
	}
}
