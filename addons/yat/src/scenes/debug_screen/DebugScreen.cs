using System;
using System.Collections.Generic;
using System.Linq;
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

    public float DefaultUpdateInterval { get; private set; } = 0.5f;

#nullable disable
    private Timer _timer;
    private VBoxContainer
        _topLeftContainer,
        _topRightContainer,
        _bottomLeftContainer,
        _bottomRightContainer;
#nullable restore

    public static readonly Dictionary<EDebugScreenItemPosition, HashSet<Tuple<string, Type>>>
    registeredItems = new()
    {
        {
            EDebugScreenItemPosition.TopLeft,
            new()
            {
                new("uid://0e2nft11f3h1", typeof(FpsItem)),
                new("uid://lfgol2xetr88", typeof(MemoryInfoItem)),
                new("uid://hvc8a2qwximn", typeof(LookingAtInfo)),
                new("uid://lscv6c8lgnyh", typeof(SceneObjectsInfo)),
            }
        },
        {
            EDebugScreenItemPosition.TopRight,
            new()
            {
                new("uid://dopcpwc6ch10v", typeof(CpuInfoItem)),
                new("uid://c4f6crgbyioh1", typeof(GpuInfoItem)),
                new("uid://ds38fns27q672", typeof(OsInfoItem)),
                new("uid://fcjyl1y5lo", typeof(EngineInfoItem)),
            }
        },
        {
            EDebugScreenItemPosition.BottomLeft,
            new()
        },
        {
            EDebugScreenItemPosition.BottomRight,
            new()
        }
    };

    public override void _Ready()
    {
        _topLeftContainer = GetNode<VBoxContainer>("%TopLeftContainer");
        _topRightContainer = GetNode<VBoxContainer>("%TopRightContainer");
        _bottomLeftContainer = GetNode<VBoxContainer>("%BottomLeftContainer");
        _bottomRightContainer = GetNode<VBoxContainer>("%BottomRightContainer");

        UpdateInterval = DefaultUpdateInterval;

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

    private void StartTimer()
    {
        _timer.WaitTime = UpdateInterval;
        _timer.Start();
    }

    private VBoxContainer? GetContainer(EDebugScreenItemPosition position)
    {
        return position switch
        {
            EDebugScreenItemPosition.TopLeft => _topLeftContainer,
            EDebugScreenItemPosition.TopRight => _topRightContainer,
            EDebugScreenItemPosition.BottomLeft => _bottomLeftContainer,
            EDebugScreenItemPosition.BottomRight => _bottomRightContainer,
            EDebugScreenItemPosition.None => throw new NotImplementedException(),
            _ => null,
        };
    }

    public void RunAll()
    {
        RemoveAllChildren();

        foreach (var (position, container) in new[]
        {
            (EDebugScreenItemPosition.TopLeft, _topLeftContainer),
            (EDebugScreenItemPosition.TopRight, _topRightContainer),
            (EDebugScreenItemPosition.BottomLeft, _bottomLeftContainer),
            (EDebugScreenItemPosition.BottomRight, _bottomRightContainer)
        })
        {
            foreach (Tuple<string, Type> item in registeredItems[position])
            {
                AddItemToContainer(item.Item1, container);
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

        IEnumerable<string> lowerTitles = titles.Select(t => t.ToLower());

        foreach (var position in new[]
        {
            EDebugScreenItemPosition.TopLeft,
            EDebugScreenItemPosition.TopRight,
            EDebugScreenItemPosition.BottomLeft,
            EDebugScreenItemPosition.BottomRight
        })
        {
            VBoxContainer? container = GetContainer(position);

            if (container == null)
            {
                continue;
            }

            foreach (string title in lowerTitles)
            {
                _ = AddItemsToContainer(registeredItems[position], container, title);
            }
        }

        StartTimer();
    }

    private static bool AddItemsToContainer(
        HashSet<Tuple<string, Type>> items,
        VBoxContainer container,
        string lowerTitle
    )
    {
        foreach (Tuple<string, Type> item in items)
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
        return item.GetCustomAttribute<TitleAttribute>()?.Title ?? string.Empty;
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
        foreach (VBoxContainer? container in new[] {
            _topLeftContainer,
            _topRightContainer,
            _bottomLeftContainer,
            _bottomRightContainer
        })
        {
            foreach (Node? child in container.GetChildren())
            {
                (child as IDebugScreenItem)!.Update();
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
        if (!item.HasInterface<IDebugScreenItem>())
        {
            return false;
        }

        if (item.GetAttribute<TitleAttribute>() is not TitleAttribute title)
        {
            return false;
        }

        return !string.IsNullOrEmpty(title.Title)
            && registeredItems[position].Add(new(path, item));
    }

    public static bool UnregisterItem(Type item, string path, EDebugScreenItemPosition position)
    {
        return registeredItems[position].Remove(new(path, item));
    }
}
