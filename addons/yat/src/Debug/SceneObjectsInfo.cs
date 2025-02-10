using System.Globalization;
using Godot;
using YAT.Attributes;

using static Godot.Performance;

namespace YAT.Debug;

[Title("SceneObjects")]
public partial class SceneObjectsInfo : DebugScreenItem
{
#nullable disable
    private RichTextLabel _label;
#nullable restore

    public override void _Ready()
    {
        base._Ready();

        _label = CreateLabel(new(0.819608f, 0.627451f, 0.305882f, 1));
        VContainer.AddChild(_label);
    }

    public override void Update()
    {
        int objects = (int)GetMonitor(Monitor.ObjectCount);
        int nodes = (int)GetMonitor(Monitor.ObjectNodeCount);
        int orphans = (int)GetMonitor(Monitor.ObjectOrphanNodeCount);
        int resources = (int)GetMonitor(Monitor.ObjectResourceCount);

        _label.Text = string.Format(
            CultureInfo.InvariantCulture,
            "Objects: {0}\nResources: {1}\nNodes: {2}\nOrphans: {3}",
            objects,
            resources,
            nodes,
            orphans
        );
    }
}
