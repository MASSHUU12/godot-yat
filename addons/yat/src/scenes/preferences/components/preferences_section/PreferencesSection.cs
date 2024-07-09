using Godot;

namespace YAT.Scenes;

public partial class PreferencesSection : PanelContainer
{
#nullable disable
    public Label Title { get; private set; }
    public VBoxContainer Content { get; private set; }
#nullable restore

    public override void _Ready()
    {
        Title = GetNode<Label>("%Title");
        Content = GetNode<VBoxContainer>("%Content");
    }
}
