using Godot;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Scenes.Monitor;

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
		var result = World.RayCast(_yat.GetViewport(), RAY_LENGTH);

		if (result is null)
		{
			_label.Text = prefix + "No camera";
			return;
		}

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
}
