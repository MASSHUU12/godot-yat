using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;

namespace YAT.Debug;

public partial class DebugScreen : Control
{
    [Export(PropertyHint.Range, "0.05, 5, 0.1")]
    public float UpdateInterval { get; set; } = DefaultUpdateInterval;
    public const float DefaultUpdateInterval = 0.5f;

#nullable disable
    private Timer _timer;
    private Dictionary<EDebugScreenItemPosition, VBoxContainer> _containers;
#nullable restore

    public static readonly Dictionary<EDebugScreenItemPosition, HashSet<Type>>
    RegisteredItems = new()
    {
        {
            EDebugScreenItemPosition.TopLeft,
            [
                typeof(FpsItem),
                typeof(MemoryInfoItem),
                typeof(LookingAtInfo),
                typeof(SceneObjectsInfo)
            ]
        },
        {
            EDebugScreenItemPosition.TopRight,
            [
                typeof(CpuInfoItem),
                typeof(GpuInfoItem),
                typeof(OsInfoItem),
                typeof(EngineInfoItem)
            ]
        },
        { EDebugScreenItemPosition.BottomLeft, new() },
        { EDebugScreenItemPosition.BottomRight, new() }
    };

    public override void _Ready()
    {
        InitializeContainers();
        RemoveAllChildren();
        InitializeTimer();
    }

    private void InitializeContainers()
    {
        _containers = new()
        {
            {
                EDebugScreenItemPosition.TopLeft,
                GetNode<VBoxContainer>("%TopLeftContainer")
            },
            {
                EDebugScreenItemPosition.TopRight,
                GetNode<VBoxContainer>("%TopRightContainer")
            },
            {
                EDebugScreenItemPosition.BottomLeft,
                GetNode<VBoxContainer>("%BottomLeftContainer")
            },
            {
                EDebugScreenItemPosition.BottomRight,
                GetNode<VBoxContainer>("%BottomRightContainer")
            }
        };
    }

    private void InitializeTimer()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.WaitTime = UpdateInterval;
        _timer.Timeout += OnTimerTimeout;
        _timer.Stop();
    }

    private void StartTimer()
    {
        _timer.WaitTime = UpdateInterval;
        _timer.Start();
    }

    private VBoxContainer? GetContainer(EDebugScreenItemPosition position)
    {
        return _containers.TryGetValue(position, out VBoxContainer? container)
            ? container
            : null;
    }

    public void RunAll()
    {
        RemoveAllChildren();

        foreach (
            (EDebugScreenItemPosition position, VBoxContainer container)
            in _containers
        )
        {
            foreach (Type item in RegisteredItems[position])
            {
                AddItemToContainer(item, container);
            }
        }

        StartTimer();
    }

    public void RunSelected(params string[] titles)
    {
        RemoveAllChildren();

        if (titles.Length == 0)
        {
            _timer.Stop();
            return;
        }

        IEnumerable<string> lowerTitles = titles.Select(
            static t => t.ToLowerInvariant()
        );

        foreach (
            (EDebugScreenItemPosition position, VBoxContainer container)
            in _containers
        )
        {
            foreach (string title in lowerTitles)
            {
                _ = AddItemsToContainer(RegisteredItems[position], container, title);
            }
        }

        StartTimer();
    }

    private static bool AddItemsToContainer(
        HashSet<Type> items,
        VBoxContainer container,
        string lowerTitle
    )
    {
        foreach (Type item in items)
        {
            if (
                GetTitle(item).Equals(
                    lowerTitle,
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                AddItemToContainer(item, container);
                return true;
            }
        }

        return false;
    }

    private static void AddItemToContainer(Type type, VBoxContainer container)
    {
        container.AddChild(CreateItem(type) as Node);
    }

    private static string GetTitle(Type item)
    {
        return item.GetCustomAttribute<TitleAttribute>()?.Title ?? string.Empty;
    }

    private static DebugScreenItem CreateItem(Type type)
    {
        return (DebugScreenItem)Activator.CreateInstance(type)!;
    }

    private void RemoveAllChildren()
    {
        foreach (VBoxContainer container in _containers.Values)
        {
            RemoveContainerChildren(container);
        }
    }

    private void OnTimerTimeout()
    {
        foreach (VBoxContainer container in _containers.Values)
        {
            foreach (Node? child in container.GetChildren())
            {
                (child as DebugScreenItem)!.Update();
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

    public static bool RegisterItem(
        Type item,
        EDebugScreenItemPosition position
    )
    {
        if (
            !item.HasInterface<DebugScreenItem>()
            || string.IsNullOrEmpty(GetTitle(item))
        )
        {
            return false;
        }

        return RegisteredItems[position].Add(item);
    }

    public static bool UnregisterItem(
        Type item,
        EDebugScreenItemPosition position
    )
    {
        return RegisteredItems[position].Remove(item);
    }
}
