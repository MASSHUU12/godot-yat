using Godot;

namespace YAT.Scenes;

public partial class PreferencesTab : PanelContainer
{
	public VBoxContainer Container { get; private set; }

	public override void _Ready()
	{
		Container = GetNode<VBoxContainer>("%VBoxContainer");
	}
}
