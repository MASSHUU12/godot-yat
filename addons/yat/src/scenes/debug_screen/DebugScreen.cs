using System.Collections.Generic;
using Godot;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Scenes;

public partial class DebugScreen : Control
{
	[Export(PropertyHint.Range, "0.05, 5, 0.1")]
	public float UpdateInterval { get; set; } = 0.5f;

	private Timer _timer;
	private VBoxContainer
		_topLeftContainer,
		_topRightContainer,
		_bottomLeftContainer,
		_bottomRightContainer;

	public static readonly List<IDebugScreenItem>[] debugScreens =
	// Use EDebugScreenItemPosition enum values as indices
	new List<IDebugScreenItem>[4] { new(), new(), new(), new() };

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

		_timer = GetNode<Timer>("Timer");
		_timer.WaitTime = UpdateInterval;
		_timer.Timeout += OnTimerTimeout;
	}

	private void OnTimerTimeout()
	{
		List<Node> children = new();
		children.AddRange(_topLeftContainer.GetChildren());
		children.AddRange(_topRightContainer.GetChildren());
		children.AddRange(_bottomLeftContainer.GetChildren());
		children.AddRange(_bottomRightContainer.GetChildren());

		foreach (Node child in children) if (child is IDebugScreenItem item) item.Update();
	}

	private static void RemoveChildren(VBoxContainer container)
	{
		foreach (Node child in container.GetChildren())
		{
			container.RemoveChild(child);
			child.QueueFree();
		}
	}

	public static bool RegisterItem(Node item, EDebugScreenItemPosition position)
	{
		if (item is not IDebugScreenItem debugItem) return false;

		if (debugScreens[(int)position].Contains(debugItem)) return false;

		debugScreens[(int)position].Add(debugItem);

		return true;
	}

	public static bool UnregisterItem(IDebugScreenItem item, EDebugScreenItemPosition position)
	{
		if (!debugScreens[(int)position].Contains(item)) return false;

		debugScreens[(int)position].Remove(item);

		return true;
	}
}
