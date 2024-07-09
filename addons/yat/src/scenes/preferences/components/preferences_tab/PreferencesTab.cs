using Godot;

namespace YAT.Scenes;

public partial class PreferencesTab : PanelContainer
{
#nullable disable
    public VBoxContainer Container { get; private set; }
#nullable restore

    public override void _Ready()
    {
        Container = GetNode<VBoxContainer>("%VBoxContainer");
    }
}
