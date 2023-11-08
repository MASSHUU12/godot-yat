using System.Collections.Generic;

/// <summary>
/// Represents a Least Recently Used (LRU) cache that stores key-value pairs up to a specified capacity.
/// When the cache is full, the least recently used item is removed to make room for new items.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the cache.</typeparam>
/// <typeparam name="TValue">The type of the values in the cache.</typeparam>
public class LRUCache<TKey, TValue>
{
	private readonly int capacity;
	private readonly Dictionary<TKey, LinkedListNode<LRUItem<TKey, TValue>>> cache;
	private readonly LinkedList<LRUItem<TKey, TValue>> lruList;

	public LRUCache(int capacity)
	{
		this.capacity = capacity;
		cache = new Dictionary<TKey, LinkedListNode<LRUItem<TKey, TValue>>>(capacity);
		lruList = new LinkedList<LRUItem<TKey, TValue>>();
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
		return default;
	}

	/// <summary>
	/// Adds a new key-value pair to the cache.
	/// If the cache is already at capacity, the least recently used item is removed.
	/// </summary>
	/// <param name="key">The key of the item to add.</param>
	/// <param name="value">The value of the item to add.</param>
	public void Add(TKey key, TValue value)
	{
		if (cache.Count >= capacity)
		{
			// Remove the least recently used item
			var lastNode = lruList.Last;
			lruList.RemoveLast();
			cache.Remove(lastNode.Value.Key);
		}

		var newNode = new LinkedListNode<LRUItem<TKey, TValue>>(new(key, value));
		lruList.AddFirst(newNode);
		cache[key] = newNode;
	}
}

/// <summary>
/// Represents an item in a Least Recently Used (LRU) cache.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class LRUItem<TKey, TValue>
{
	public TKey Key { get; }
	public TValue Value { get; }

	public LRUItem(TKey key, TValue value)
	{
		Key = key;
		Value = value;
	}
}
