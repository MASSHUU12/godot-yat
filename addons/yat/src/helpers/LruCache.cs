using System.Collections.Generic;

public class LRUCache<TKey, TValue>
{
	private readonly int capacity;
	private readonly Dictionary<TKey, LinkedListNode<LRUItem<TKey, TValue>>> cache;
	private readonly LinkedList<LRUItem<TKey, TValue>> lruList;

	public LRUCache(int capacity)
	{
		this.capacity = capacity;
		this.cache = new Dictionary<TKey, LinkedListNode<LRUItem<TKey, TValue>>>(capacity);
		this.lruList = new LinkedList<LRUItem<TKey, TValue>>();
	}

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
