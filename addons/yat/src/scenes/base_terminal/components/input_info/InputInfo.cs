using Godot;

namespace YAT.Scenes;

public partial class InputInfo : PanelContainer
{
#nullable disable
    public RichTextLabel _text;
    private MarginContainer _container;
#nullable restore

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
