using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using YAT.Attributes;
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
		InitializeTimer();
	}

	private void InitializeTimer()
	{
		_timer = GetNode<Timer>("Timer");
		_timer.WaitTime = UpdateInterval;
		_timer.Timeout += OnTimerTimeout;
		_timer.Stop();
	}

	private VBoxContainer GetContainer(EDebugScreenItemPosition position)
	{
		return position switch
		{
			EDebugScreenItemPosition.TopLeft => _topLeftContainer,
			EDebugScreenItemPosition.TopRight => _topRightContainer,
			EDebugScreenItemPosition.BottomLeft => _bottomLeftContainer,
			EDebugScreenItemPosition.BottomRight => _bottomRightContainer,
			_ => null,
		};
	}

	public void RunAll()
	{
		RemoveAllChildren();

		for (int i = 0; i < registeredItems.Length; i++)
		{
			foreach (var item in registeredItems[i])
			{
				AddItemToContainer(item.Item1, GetContainer((EDebugScreenItemPosition)i));
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

			if (AddItemsToContainer(
				registeredItems[(int)EDebugScreenItemPosition.TopLeft],
				_topLeftContainer,
				lowerTitle
			)) continue;
			if (AddItemsToContainer(
				registeredItems[(int)EDebugScreenItemPosition.TopRight],
				_topRightContainer,
				lowerTitle
			)) continue;
			if (AddItemsToContainer(
				registeredItems[(int)EDebugScreenItemPosition.BottomLeft],
				_bottomLeftContainer,
				lowerTitle
			)) continue;
			if (AddItemsToContainer(
				registeredItems[(int)EDebugScreenItemPosition.BottomRight],
				_bottomRightContainer,
				lowerTitle
			)) continue;
		}

		_timer.Start();
	}

	private static bool AddItemsToContainer(
		HashSet<Tuple<string, Type>> items,
		VBoxContainer container,
		string lowerTitle
	)
	{
		foreach (var item in items)
		{
			if (GetTitle(item.Item2).ToLower() == lowerTitle)
			{
				AddItemToContainer(item.Item1, container);
				return true;
			}
		}

		return false;
	}

	private static void AddItemToContainer(string path, VBoxContainer container)
	{
		container.AddChild(CreateItem(path) as Node);
	}

	private static string GetTitle(Type item)
	{
		return item.GetCustomAttribute<TitleAttribute>().Title;
	}

	private static IDebugScreenItem CreateItem(string path)
	{
		return GD.Load<PackedScene>(path).Instantiate<IDebugScreenItem>();
	}

	private void RemoveAllChildren()
	{
		RemoveContainerChildren(_topLeftContainer);
		RemoveContainerChildren(_topRightContainer);
		RemoveContainerChildren(_bottomLeftContainer);
		RemoveContainerChildren(_bottomRightContainer);
	}

	private void OnTimerTimeout()
	{
		foreach (var container in new[] {
			_topLeftContainer,
			_topRightContainer,
			_bottomLeftContainer,
			_bottomRightContainer
		})
		{
			foreach (var child in container.GetChildren())
			{
				(child as IDebugScreenItem).Update();
			}
		}
	}

	private static void RemoveContainerChildren(VBoxContainer container)
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
		if (item.GetAttribute<TitleAttribute>() is not TitleAttribute title) return false;
		if (string.IsNullOrEmpty(title.Title)) return false;

		return registeredItems[(int)position].Add(new(path, item));
	}

	public static bool UnregisterItem(Type item, string path, EDebugScreenItemPosition position)
	{
		return registeredItems[(int)position].Remove(new(path, item));
	}
}
