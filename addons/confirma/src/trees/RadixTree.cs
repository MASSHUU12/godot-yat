using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Confirma.Trees;

/// <summary>
/// Represents a radix tree data structure, also known as a prefix tree or trie.
/// A radix tree is a tree-like data structure that is often used to store a
/// dynamic set or associative array where the keys are usually strings.
/// </summary>
/// <typeparam name="TValue">The type of the values stored in the tree.</typeparam>
public class RadixTree<TValue> : PrefixTree<TValue>
{
    public RadixNode<TValue> Root { get; } = new();

    public override void Add(string key, TValue value)
    {
        _ = GetOrAddNode(key, value);
    }

    public override void Add(ReadOnlySpan<char> key, TValue value)
    {
        _ = GetOrAddNode(key, value);
    }

    public override void Clear()
    {
        Root.Children.Clear();
        Root.Value = default;
    }

    public override bool Remove(string key)
    {
        return Remove(Root, key.AsSpan(), out bool _);
    }

    public override bool Remove(KeyValuePair<string, TValue> item)
    {
        return Remove(Root, item.Key, out bool _);
    }

    private bool Remove(
        RadixNode<TValue> node,
        ReadOnlySpan<char> key,
        out bool shouldDeleteNode
    )
    {
        shouldDeleteNode = false;

        if (key.IsEmpty)
        {
            if (node.HasValue)
            {
                // Found the node to remove
                node.Value = default;
                node.HasValue = false;
                // If node has no children, indicate it can be deleted
                shouldDeleteNode = node.Children.Count == 0;
                return true;
            }
            // Key not found
            return false;
        }

        char firstChar = key[0];

        if (!node.Children.TryGetValue(firstChar, out RadixNode<TValue>? child))
        {
            // Key not found
            return false;
        }

        ReadOnlySpan<char> label = child.Prefix.Span;
        int matchLength = CommonPrefixLength(key, label);

        if (matchLength < label.Length)
        {
            // Key not found
            return false;
        }

        ReadOnlySpan<char> remainingKey = key[matchLength..];

        bool result = Remove(child, remainingKey, out bool shouldDeleteChild);

        if (shouldDeleteChild)
        {
            _ = node.Children.Remove(firstChar);
        }

        // Check if current node should be merged or deleted
        if (node != Root && node.Value is null && node.Children.Count == 1)
        {
            RadixNode<TValue> singleChild = node.Children.Values.First();

            // Merge with the child node
            node.Prefix = Concat(node.Prefix, singleChild.Prefix);
            node.Value = singleChild.Value;
            node.HasValue = singleChild.HasValue;
            node.Children = singleChild.Children;

            foreach (RadixNode<TValue>? grandChild in node.Children.Values)
            {
                grandChild.Parent = node;
            }
        }

        shouldDeleteNode = node != Root
            && node.Value is null
            && node.Children.Count == 0;
        return result;
    }

    public override IEnumerable<KeyValuePair<string, TValue>> Search(string prefix)
    {
        RadixNode<TValue>? node = Root;
        ReadOnlySpan<char> remainingPrefix = prefix.AsSpan();
        Stack<(RadixNode<TValue> Node, string KeySoFar)> stack = new();

        while (!remainingPrefix.IsEmpty)
        {
            char firstChar = remainingPrefix[0];

            if (!node.Children.TryGetValue(firstChar, out RadixNode<TValue>? child))
            {
                // No matching prefix
                yield break;
            }

            ReadOnlySpan<char> label = child.Prefix.Span;
            int matchLength = CommonPrefixLength(remainingPrefix, label);

            if (matchLength < label.Length)
            {
                // Prefix doesn't fully match
                yield break;
            }

            remainingPrefix = remainingPrefix[matchLength..];
            node = child;
        }

        List<char> keySoFar = new(prefix);

        // Perform DFS to collect all nodes under the prefix
        stack.Push((node, prefix));

        while (stack.Count > 0)
        {
            (RadixNode<TValue> currentNode, string accumulatedKey) = stack.Pop();

            if (currentNode.HasValue)
            {
                yield return new(new(accumulatedKey.ToArray()), currentNode.Value!);
            }

            foreach (RadixNode<TValue> child in currentNode.Children.Values.OrderBy(static n => n.Prefix.Span[0]))
            {
                // Create a new list to avoid modifying the current accumulated key
                StringBuilder childKey = new(accumulatedKey);
                _ = childKey.Append(child.Prefix);

                stack.Push((child, childKey.ToString()));
            }
        }
    }

    public override bool Lookup(string x)
    {
        RadixNode<TValue> node = Root;
        ReadOnlySpan<char> remainingKey = x.AsSpan();

        while (!remainingKey.IsEmpty)
        {
            char firstChar = remainingKey[0];

            if (!node.Children.TryGetValue(firstChar, out RadixNode<TValue>? child))
            {
                // No matching child, key doesn't exist
                return false;
            }

            ReadOnlySpan<char> label = child.Prefix.Span;
            int matchLength = CommonPrefixLength(remainingKey, label);

            if (matchLength < label.Length)
            {
                // Partial match, key doesn't exist
                return false;
            }

            remainingKey = remainingKey[matchLength..];
            node = child;
        }

        return node.IsLeaf();
    }

    public override bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value)
    {
        RadixNode<TValue> node = Root;
        ReadOnlySpan<char> remainingKey = key.AsSpan();

        while (!remainingKey.IsEmpty)
        {
            char firstChar = remainingKey[0];

            if (!node.Children.TryGetValue(firstChar, out RadixNode<TValue>? child))
            {
                value = default;
                return false;
            }

            ReadOnlySpan<char> label = child.Prefix.Span;
            int matchLength = CommonPrefixLength(remainingKey, label);

            if (matchLength < label.Length)
            {
                value = default;
                return false;
            }

            remainingKey = remainingKey[matchLength..];
            node = child;
        }

        if (node.HasValue)
        {
            value = node.Value!;
            return true;
        }

        value = default;
        return false;
    }

    public RadixNode<TValue>? FindSuccessor(string key)
    {
        RadixNode<TValue>? node = Root;
        ReadOnlySpan<char> remainingKey = key.AsSpan();
        string accumulatedKey = string.Empty;

        while (!remainingKey.IsEmpty)
        {
            char firstChar = remainingKey[0];

            if (!node.Children.TryGetValue(firstChar, out RadixNode<TValue>? child))
            {
                break; // No exact match; need to find the next node in order
            }

            ReadOnlySpan<char> label = child.Prefix.Span;
            int matchLength = CommonPrefixLength(remainingKey, label);

            accumulatedKey += label[..matchLength].ToString();
            remainingKey = remainingKey[matchLength..];

            if (matchLength < label.Length)
            {
                // Partial match; explore this child for successor
                node = child;
                break;
            }

            node = child;
        }

        // If the node has a value and it's not the key itself, return it
        if (node.HasValue && string.CompareOrdinal(node.GetFullKey(), key) > 0)
        {
            return node;
        }

        // Perform in-order traversal to find the successor
        return FindNextNode(node);
    }

    private static RadixNode<TValue>? FindNextNode(RadixNode<TValue>? node)
    {
        // If there are children, find the leftmost (smallest) child
        if (node?.Children.Count > 0)
        {
            RadixNode<TValue> child = node.Children.Values
                .OrderBy(n => n.Prefix.Span[0]).First();

            // Continue down the leftmost path
            while (child.Children.Count > 0)
            {
                if (child.HasValue)
                {
                    return child;
                }
                child = child.Children.Values.OrderBy(n => n.Prefix.Span[0]).First();
            }
            return child.HasValue ? child : null;
        }

        // Traverse up to find the next successor
        RadixNode<TValue>? parent = node?.Parent;
        RadixNode<TValue>? current = node;

        while (parent is not null)
        {
            IOrderedEnumerable<RadixNode<TValue>> siblings = parent.Children.Values
                .Where(n =>
                    n != current
                    && string.CompareOrdinal(
                        n.Prefix.ToString(),
                        current?.Prefix.ToString()
                    ) > 0
                ).OrderBy(n => n.Prefix.ToString());

            if (siblings.Any())
            {
                RadixNode<TValue> nextSibling = siblings.First();
                return FindLeftmost(nextSibling);
            }

            current = parent;
            parent = parent.Parent;
        }

        return null;
    }

    private static RadixNode<TValue>? FindLeftmost(RadixNode<TValue> node)
    {
        while (true)
        {
            if (node.HasValue)
            {
                return node;
            }

            if (node.Children.Count == 0)
            {
                return null;
            }

            node = node.Children.Values.OrderBy(static n => n.Prefix.Span[0]).First();
        }
    }

    public RadixNode<TValue>? FindPredecessor(string key)
    {
        RadixNode<TValue>? node = Root;
        ReadOnlySpan<char> remainingKey = key.AsSpan();
        RadixNode<TValue>? lastValueNode = null;

        // Traverse the tree to find the node matching the key or the closest ancestor
        while (!remainingKey.IsEmpty)
        {
            char firstChar = remainingKey[0];

            var smallerChildren = node.Children
                .Where(c => c.Key < firstChar)
                .OrderByDescending(c => c.Key);

            if (smallerChildren.Any())
            {
                return FindRightmost(smallerChildren.First().Value);
            }

            if (!node.Children.TryGetValue(firstChar, out RadixNode<TValue>? child))
            {
                // No exact match; need to backtrack to find predecessor
                break;
            }

            ReadOnlySpan<char> label = child.Prefix.Span;
            int matchLength = CommonPrefixLength(remainingKey, label);

            if (matchLength < label.Length)
            {
                // Partial match; need to consider left subtree
                if (matchLength < remainingKey.Length && label[matchLength] < remainingKey[matchLength])
                {
                    // Child nodes might have predecessors
                    return FindRightmost(child);
                }
                break;
            }

            remainingKey = remainingKey[matchLength..];
            node = child;

            if (node.HasValue)
            {
                // Keep track of the last node with a value encountered
                lastValueNode = node;
            }
        }

        // If we have a node with a value less than the key, return it
        if (lastValueNode is not null && string.CompareOrdinal(lastValueNode.GetFullKey(), key) < 0)
        {
            return lastValueNode;
        }

        // Traverse up to find the predecessor if necessary
        return FindPreviousNode(node);
    }

    private static RadixNode<TValue>? FindPreviousNode(RadixNode<TValue> node)
    {
        RadixNode<TValue>? parent = node.Parent;
        RadixNode<TValue>? current = node;

        while (parent is not null)
        {
            // Find siblings that come before the current node
            var siblings = parent.Children
                .Where(c => c.Value != current && c.Key < current.Prefix.Span[0])
                .OrderByDescending(c => c.Key);

            if (siblings.Any())
            {
                RadixNode<TValue>? predecessorSibling = siblings.First().Value;
                return FindRightmost(predecessorSibling);
            }

            if (parent.HasValue)
            {
                return parent;
            }

            current = parent;
            parent = parent.Parent;
        }

        return null;
    }

    private static RadixNode<TValue>? FindRightmost(RadixNode<TValue> node)
    {
        while (true)
        {
            if (node.Children.Count == 0)
            {
                return node.HasValue ? node : null;
            }

            // Get the child with the greatest key (rightmost child)
            node = node.Children.Values
                .OrderByDescending(static n => n.Prefix.Span[0])
                .First();

            if (node.HasValue)
            {
                // Continue down to find the rightmost value node
                RadixNode<TValue>? potentialNode = RadixTree<TValue>.FindRightmost(node);
                return potentialNode ?? node;
            }
        }
    }

    private static int CommonPrefixLength(
        ReadOnlySpan<char> span1,
        ReadOnlySpan<char> span2
    )
    {
        int minLength = Math.Min(span1.Length, span2.Length);
        int i = 0;
        while (i < minLength && span1[i] == span2[i])
        {
            i++;
        }
        return i;
    }

    private static ReadOnlyMemory<char> Concat(
        ReadOnlyMemory<char> prefix1,
        ReadOnlyMemory<char> prefix2
    )
    {
        char[] newPrefix = new char[prefix1.Length + prefix2.Length];
        prefix1.Span.CopyTo(newPrefix);
        prefix2.Span.CopyTo(newPrefix.AsSpan(prefix1.Length));
        return new ReadOnlyMemory<char>(newPrefix);
    }

    private RadixNode<TValue> GetOrAddNode(
        ReadOnlySpan<char> key,
        TValue value
    )
    {
        RadixNode<TValue> node = Root;
        ReadOnlySpan<char> remainingKey = key;

        while (true)
        {
            if (remainingKey.IsEmpty)
            {
                // End of the key reached, assing the value
                node.Value = value;
                return node;
            }

            char firstChar = remainingKey[0];

            if (!node.Children.TryGetValue(firstChar, out RadixNode<TValue>? child))
            {
                // Create a new node with the remaining key
                RadixNode<TValue> n = new(remainingKey.ToArray())
                {
                    Value = value,
                    HasValue = true,
                    Parent = node
                };
                node.Children[firstChar] = n;
                return n;
            }

            ReadOnlySpan<char> label = child.Prefix.Span;
            int matchLength = CommonPrefixLength(remainingKey, label);

            if (matchLength == label.Length)
            {
                // Full match of the label
                remainingKey = remainingKey[matchLength..];
                node = child;
                continue;
            }

            // Partial match, need to split the node
            char[] commonPrefix = remainingKey[..matchLength].ToArray();
            RadixNode<TValue> splitNode = new(commonPrefix)
            {
                Value = value,
                HasValue = true,
                Parent = node
            };
            node.Children[firstChar] = splitNode;

            // Adjust the existing child
            char[] childSuffix = label[matchLength..].ToArray();
            child.Prefix = childSuffix;
            child.Parent = splitNode;
            splitNode.Children[childSuffix[0]] = child;

            if (matchLength == remainingKey.Length)
            {
                // Remaining key matches the split point
                splitNode.Value = value;
                return splitNode;
            }

            // Create a new node for the remaining key
            char[] suffix = remainingKey[matchLength..].ToArray();
            RadixNode<TValue> newNode = new(suffix)
            {
                Value = value,
                HasValue = true,
                Parent = splitNode
            };
            splitNode.Children[suffix[0]] = newNode;

            return newNode;
        }
    }
}
