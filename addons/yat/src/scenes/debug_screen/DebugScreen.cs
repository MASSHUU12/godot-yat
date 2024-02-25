using Godot;

namespace YAT.Scenes;

public partial class DebugScreen : CanvasLayer
{
	private VBoxContainer
		_topLeftContainer,
		_topRightContainer,
		_bottomLeftContainer,
		_bottomRightContainer;

	public override void _Ready()
	{
		_topLeftContainer = GetNode<VBoxContainer>("%TopLeftContainer");
		_topRightContainer = GetNode<VBoxContainer>("%TopRightContainer");
		_bottomLeftContainer = GetNode<VBoxContainer>("%BottomLeftContainer");
		_bottomRightContainer = GetNode<VBoxContainer>("%BottomRightContainer");

		RemoveChildren(_topLeftContainer);
		RemoveChildren(_topRightContainer);
		RemoveChildren(_bottomLeftContainer);
		RemoveChildren(_bottomRightContainer);
	}

	private static void RemoveChildren(VBoxContainer container)
	{
		foreach (Node child in container.GetChildren())
		{
			container.RemoveChild(child);
			child.QueueFree();
		}
	}
}
