using Godot;

namespace YAT.Scenes;

[Tool]
public partial class EditorTerminal : Control
{
#nullable disable
	public BaseTerminal BaseTerminal { get; private set; }
#nullable restore

	public override void _Ready()
	{
		BaseTerminal = GetNode<BaseTerminal>("Content/BaseTerminal");
	}
}
