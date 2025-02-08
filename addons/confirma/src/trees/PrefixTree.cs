using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Confirma.Trees;

public abstract class PrefixTree<TValue> : IDictionary<string, TValue>
{
    public virtual TValue this[string key]
    {
        get => TryGetValue(key, out TValue? value)
            ? value
            : throw new KeyNotFoundException();
        set => Add(key, value);
    }

    public IEnumerable<string> Keys => this.Select(static t => t.Key);
    public IEnumerable<TValue> Values => this.Select(static t => t.Value);

    ICollection<string> IDictionary<string, TValue>.Keys => Keys.ToList();
    ICollection<TValue> IDictionary<string, TValue>.Values => Values.ToList();

    public virtual int Count => Keys.Count();
    public virtual bool IsReadOnly => false;

    public abstract void Add(string key, TValue value);
    public abstract void Add(ReadOnlySpan<char> key, TValue value);
    public abstract void Clear();
    public abstract bool Remove(string key);
    public abstract bool Remove(KeyValuePair<string, TValue> item);
    public abstract IEnumerable<KeyValuePair<string, TValue>> Search(string prefix);
    public abstract bool Lookup(string x);
    public abstract bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value);

    public virtual bool ContainsKey(string key)
    {
        return TryGetValue(key, out _);
    }

    public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
    {
        foreach (KeyValuePair<string, TValue> kv in this)
        {
            array[arrayIndex++] = kv;
        }
    }

    public void Add(KeyValuePair<string, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public bool Contains(KeyValuePair<string, TValue> item)
    {
        return TryGetValue(item.Key, out TValue? value)
            && value?.Equals(item.Value) == true;
    }

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
        return Search(string.Empty).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
