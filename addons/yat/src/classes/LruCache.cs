using System;
using System.Collections.Generic;

namespace YAT.Classes;

public class LRUCache<TKey, TValue> where TKey : notnull where TValue : notnull
{
    private readonly ushort capacity;
    private readonly Dictionary<TKey, LinkedListNode<LRUItem<TKey, TValue>>> cache;
    private readonly LinkedList<LRUItem<TKey, TValue>> lruList;

    public LRUCache(ushort capacity)
    {
        if (capacity == 0)
        {
            throw new ArgumentException("Capacity must be greater than 0.", nameof(capacity));
        }

        this.capacity = capacity;
        cache = new(capacity);
        lruList = new();
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="key">The key of the value to get.</param>
    /// <returns>The value associated with the specified key, or the default value of <typeparamref name="TValue"/> if the key is not found.</returns>
    public TValue Get(TKey key)
    {
        if (cache.TryGetValue(key, out var node))
        {
            // Move the accessed item to the front of the list
            lruList.Remove(node);
            lruList.AddFirst(node);
            return node.Value.Value;
        }
        return default!;
    }

    /// <summary>
    /// Adds a new key-value pair to the cache.
    /// If the cache is already at capacity, the least recently used item is removed.
    /// </summary>
    /// <param name="key">The key of the item to add.</param>
    /// <param name="value">The value of the item to add.</param>
    public void Add(TKey key, TValue value)
    {
        if (cache.Count >= capacity && lruList.Last is not null)
        {
            // Remove the least recently used item
            var lastNode = lruList.Last;
            lruList.RemoveLast();
            _ = cache.Remove(lastNode!.Value.Key);
        }

        lruList.AddFirst(new LinkedListNode<LRUItem<TKey, TValue>>(new(key, value)));
        cache[key] = lruList.First!;
    }

    public int Size => cache.Count;
}

public class LRUItem<TKey, TValue> where TKey : notnull where TValue : notnull
{
    public TKey Key { get; }
    public TValue Value { get; }

    public LRUItem(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}
