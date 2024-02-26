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

	public static readonly List<IDebugScreenItem>[] registeredItems =
	// Use EDebugScreenItemPosition enum values as indices
	new List<IDebugScreenItem>[4] {
		new() // Top Left
		{
			GD.Load<PackedScene>("uid://0e2nft11f3h1").Instantiate<FpsItem>(),
			GD.Load<PackedScene>("uid://lfgol2xetr88").Instantiate<MemoryInfoItem>(),
			GD.Load<PackedScene>("uid://hvc8a2qwximn").Instantiate<LookingAtInfo>(),
			GD.Load<PackedScene>("uid://lscv6c8lgnyh").Instantiate<SceneObjectsInfo>(),
		},
		new() // Top Right
		{
			GD.Load<PackedScene>("uid://dopcpwc6ch10v").Instantiate<CpuInfoItem>(),
			GD.Load<PackedScene>("uid://c4f6crgbyioh1").Instantiate<GpuInfoItem>(),
			GD.Load<PackedScene>("uid://ds38fns27q672").Instantiate<OsInfoItem>(),
			GD.Load<PackedScene>("uid://fcjyl1y5lo").Instantiate<EngineInfoItem>(),
		},
		new(), // Bottom Left
		new()  // Bottom Right
	};

	public override void _Ready()
	{
		_topLeftContainer = GetNode<VBoxContainer>("%TopLeftContainer");
		_topRightContainer = GetNode<VBoxContainer>("%TopRightContainer");
		_bottomLeftContainer = GetNode<VBoxContainer>("%BottomLeftContainer");
		_bottomRightContainer = GetNode<VBoxContainer>("%BottomRightContainer");

		RemoveAllChildren();

		_timer = GetNode<Timer>("Timer");
		_timer.WaitTime = UpdateInterval;
		_timer.Timeout += OnTimerTimeout;
	}

	public void RunAll()
	{
		RemoveAllChildren();

		for (int i = 0; i < registeredItems.Length; i++)
		{
			foreach (IDebugScreenItem item in registeredItems[i])
			{
				switch (i)
				{
					case (int)EDebugScreenItemPosition.TopLeft:
						_topLeftContainer.AddChild((item as Node).Duplicate());
						break;
					case (int)EDebugScreenItemPosition.TopRight:
						_topRightContainer.AddChild((item as Node).Duplicate());
						break;
					case (int)EDebugScreenItemPosition.BottomLeft:
						_bottomLeftContainer.AddChild((item as Node).Duplicate());
						break;
					case (int)EDebugScreenItemPosition.BottomRight:
						_bottomRightContainer.AddChild((item as Node).Duplicate());
						break;
				}
			}
		}
	}

	public void RunSelected(params string[] titles)
	{
		RemoveAllChildren();

		if (titles.Length == 0) return;

		foreach (string title in titles)
		{
			var lowerTitle = title.ToLower();

			AddItemToContainer(
				registeredItems[(int)EDebugScreenItemPosition.TopLeft],
				_topLeftContainer,
				lowerTitle
			);
			AddItemToContainer(
				registeredItems[(int)EDebugScreenItemPosition.TopRight],
				_topRightContainer,
				lowerTitle
			);
			AddItemToContainer(
				registeredItems[(int)EDebugScreenItemPosition.BottomLeft],
				_bottomLeftContainer,
				lowerTitle
			);
			AddItemToContainer(
				registeredItems[(int)EDebugScreenItemPosition.BottomRight],
				_bottomRightContainer,
				lowerTitle
			);
		}
	}

	private static void AddItemToContainer(
		List<IDebugScreenItem> items,
		VBoxContainer container,
		string lowerTitle
	)
	{
		foreach (IDebugScreenItem item in items)
		{
			if (item.Title.ToLower() == lowerTitle)
			{
				container.AddChild((item as Node).Duplicate());
				break;
			}
		}
	}

	private void RemoveAllChildren()
	{
		RemoveChildren(_topLeftContainer);
		RemoveChildren(_topRightContainer);
		RemoveChildren(_bottomLeftContainer);
		RemoveChildren(_bottomRightContainer);
	}

	private void OnTimerTimeout()
	{
		List<Node> children = new();
		children.AddRange(_topLeftContainer.GetChildren());
		children.AddRange(_topRightContainer.GetChildren());
		children.AddRange(_bottomLeftContainer.GetChildren());
		children.AddRange(_bottomRightContainer.GetChildren());

		foreach (Node child in children) (child as IDebugScreenItem)?.Update();
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

		if (registeredItems[(int)position].Contains(debugItem)) return false;

		registeredItems[(int)position].Add(debugItem);

		return true;
	}

	public static bool UnregisterItem(IDebugScreenItem item, EDebugScreenItemPosition position)
	{
		if (!registeredItems[(int)position].Contains(item)) return false;

		registeredItems[(int)position].Remove(item);

		return true;
	}
}
