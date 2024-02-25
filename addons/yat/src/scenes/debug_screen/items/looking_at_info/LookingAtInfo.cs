using Godot;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Scenes;

public partial class LookingAtInfo : PanelContainer, IDebugScreenItem
{
	public string Title { get; set; } = "LookingAt";

	private YAT _yat;
	private Label _label;
	private const uint RAY_LENGTH = 1024;

	private const string PREFIX = "Looking at: ";
	private const string NOTHING = PREFIX + "Nothing";
	private const string NO_CAMERA = PREFIX + "No camera";

	public override void _Ready()
	{
		_yat = GetNode<YAT>("/root/YAT");
		_label = GetNode<Label>("Label");
	}

	public void Update()
	{
		var result = World.RayCast(_yat.GetViewport(), RAY_LENGTH);

		if (result is null)
		{
			_label.Text = NO_CAMERA;
			return;
		}

		if (result.Count == 0)
		{
			_label.Text = NOTHING;
			return;
		}

		Node node = result["collider"].As<Node>();
		Vector2 position = result["position"].As<Vector2>();

		if (node is null)
		{
			_label.Text = NOTHING;
			return;
		}

		_label.Text = PREFIX + node.Name + " at " + position;
	}
}
