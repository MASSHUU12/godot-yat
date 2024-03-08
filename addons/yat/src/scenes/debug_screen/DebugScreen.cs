using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using YAT.Enums;
using YAT.Helpers;
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

	public static readonly HashSet<Tuple<string, Type>>[] registeredItems =
	// Use EDebugScreenItemPosition enum values as indices
	new HashSet<Tuple<string, Type>>[4] {
		new() // Top Left
		{
			new("uid://0e2nft11f3h1", typeof(FpsItem)),
			new("uid://lfgol2xetr88", typeof(MemoryInfoItem)),
			new("uid://hvc8a2qwximn", typeof(LookingAtInfo)),
			new("uid://lscv6c8lgnyh", typeof(SceneObjectsInfo)),
		},
		new() // Top Right
		{
			new("uid://dopcpwc6ch10v", typeof(CpuInfoItem)),
			new("uid://c4f6crgbyioh1", typeof(GpuInfoItem)),
			new("uid://ds38fns27q672", typeof(OsInfoItem)),
			new("uid://fcjyl1y5lo", typeof(EngineInfoItem)),
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
		_timer.Stop();
	}

	public void RunAll()
	{
		RemoveAllChildren();

		for (int i = 0; i < registeredItems.Length; i++)
		{
			foreach (IDebugScreenItem item in registeredItems[i].Cast<IDebugScreenItem>())
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

		_timer.Start();
	}

	public void RunSelected(params string[] titles)
	{
		RemoveAllChildren();

		if (titles.Length == 0)
		{
			_timer.Stop();
			return;
		}

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

		_timer.Start();
	}

	private static void AddItemsToContainer(
		HashSet<Tuple<string, Type>> items,
		VBoxContainer container,
		string lowerTitle
	)
	{
		foreach (var item in items)
		{
			if (item.Title.ToLower() == lowerTitle)
			{
				container.AddChild((item as Node).Duplicate());
				break;
			}
		}
	}

	private static string GetTitle(Type item)
	{
		var @interface = item.GetInterface(nameof(IDebugScreenItem));

		if (@interface == null) return string.Empty;

		return @interface.GetProperty("Title").GetValue() as string;
	}

	private static IDebugScreenItem CreateItem(Type item, string path)
	{
		return GD.Load<PackedScene>(path).Instantiate<IDebugScreenItem>();
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

		GD.Print("DebugScreen updated");
	}

	private static void RemoveChildren(VBoxContainer container)
	{
		foreach (Node child in container.GetChildren())
		{
			container.RemoveChild(child);
			child.QueueFree();
		}
	}

	public static bool RegisterItem(Type item, string path, EDebugScreenItemPosition position)
	{
		if (!item.HasInterface<IDebugScreenItem>()) return false;

		return registeredItems[(int)position].Add(new(path, item));
	}

	public static bool UnregisterItem(Type item, string path, EDebugScreenItemPosition position)
	{
		return registeredItems[(int)position].Remove(new(path, item));
	}
}
