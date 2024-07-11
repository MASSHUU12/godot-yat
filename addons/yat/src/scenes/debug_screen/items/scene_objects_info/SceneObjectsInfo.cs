using Godot;
using YAT.Attributes;
using YAT.Interfaces;

namespace YAT.Scenes;

[Title("SceneObjects")]
public partial class SceneObjectsInfo : PanelContainer, IDebugScreenItem
{
#nullable disable
    private Label _label;
#nullable restore

    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
    }

    public void Update()
    {
        double objects = Performance.GetMonitor(Performance.Monitor.ObjectCount);
        double nodes = Performance.GetMonitor(Performance.Monitor.ObjectNodeCount);
        double orphans = Performance.GetMonitor(Performance.Monitor.ObjectOrphanNodeCount);
        double resources = Performance.GetMonitor(Performance.Monitor.ObjectResourceCount);

        _label.Text = string.Format("Objects: {0}\nResources: {1}\nNodes: {2}\nOrphans: {3}",
            objects,
            resources,
            nodes,
            orphans
        );
    }
}
