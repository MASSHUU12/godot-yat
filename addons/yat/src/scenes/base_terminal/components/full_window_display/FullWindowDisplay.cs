using Godot;
using YAT.Helpers;

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

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed(Keybindings.TerminalCloseFullWindowDisplay))
			Close();
	}

	public void Open(string text)
	{
		MainDisplay.Clear();
		MainDisplay.CallDeferred("append_text", text);
		Visible = true;
	}

	public void Close()
	{
		MainDisplay.Clear();
		Visible = false;
	}
}
