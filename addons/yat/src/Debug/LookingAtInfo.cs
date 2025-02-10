using Godot;
using Godot.Collections;
using YAT.Attributes;
using YAT.Helpers;

namespace YAT.Debug;

[Title("LookingAt")]
public partial class LookingAtInfo : DebugScreenItem
{
#nullable disable
    private RichTextLabel _label;
#nullable restore
    private const uint RAY_LENGTH = 1024;

    private const string PREFIX = "Looking at: ";
    private const string NOTHING = PREFIX + "Nothing";
    private const string NO_CAMERA = PREFIX + "No camera";

    public override void _Ready()
    {
        base._Ready();

        _label = CreateLabel();
        VContainer.AddChild(_label);
    }

    public override void Update()
    {
        Dictionary? result = World.RayCast(Yat.GetViewport(), RAY_LENGTH);

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
