using Godot;

namespace YAT.Scenes;

public partial class FullWindowDisplay : Control
{
	public RichTextLabel MainDisplay { get; private set; }
	public Label HelpLabel { get; private set; }

	public override void _Ready()
	{
		MainDisplay = GetNode<RichTextLabel>("%MainDisplay");
		HelpLabel = GetNode<Label>("%HelpLabel");
	}
}
