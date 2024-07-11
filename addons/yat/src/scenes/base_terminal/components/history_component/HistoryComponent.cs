using System.Collections.Generic;
using Godot;

namespace YAT.Scenes;

public partial class HistoryComponent : Node
{
    public readonly LinkedList<string> History = new();
    public LinkedListNode<string>? CurrentNode { get; private set; }

    private YAT? _yat;

    public override void _Ready()
    {
        _yat = GetNode<YAT>("/root/YAT");
    }

    public void Add(string command)
    {
        if (History.Count >= _yat!.PreferencesManager.Preferences.HistoryLimit)
        {
            return;
        }

        CurrentNode = null;
        _ = History.AddLast(command);
    }

    public LinkedListNode<string>? MovePrevious()
    {
        if (CurrentNode is null && History.Count > 0)
        {
            CurrentNode = History.Last;
            return CurrentNode;
        }
        else if (CurrentNode?.Previous is not null)
        {
            CurrentNode = CurrentNode.Previous;
            return CurrentNode;
        }

        return null;
    }

    public LinkedListNode<string>? MoveNext()
    {
        if (CurrentNode?.Next is not null)
        {
            CurrentNode = CurrentNode.Next;
            return CurrentNode;
        }

        CurrentNode = null;
        return null;
    }
}
