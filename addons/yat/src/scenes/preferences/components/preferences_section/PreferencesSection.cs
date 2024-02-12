using Godot;

namespace YAT.Scenes;

public partial class PreferencesSection : PanelContainer
{
	public Label Title { get; private set; }
	public VBoxContainer Content { get; private set; }

	public override void _Ready()
	{
		Title = GetNode<Label>("%Title");
		Content = GetNode<VBoxContainer>("%Content");
	}
}
