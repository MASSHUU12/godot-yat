using Godot;

namespace YAT.Scenes;

public partial class InputInfo : PanelContainer
{
	public RichTextLabel _text;
	private MarginContainer _container;

	public override void _Ready()
	{
		_text = GetNode<RichTextLabel>("%Text");
		_container = GetNode<MarginContainer>("./MarginContainer");
		Visible = false;
	}

	public void DisplayCommandInfo(string text)
	{
		_text.Text = text;
		Visible = true;
	}
}
