using System;
using System.Collections.Generic;
using System.Linq;

namespace Confirma.Helpers;

public static class ArrayHelper
{
    public static string ToString(object?[]? array, uint depth = 0, uint maxDepth = 1)
    {
        if (depth > maxDepth) return string.Empty;
        if (array is null || array.Length == 0) return string.Empty;

        List<string> list = new();

        foreach (object? item in array)
        {
            if (item is null)
            {
                list.Add("null");
                continue;
            }

            if (item.GetType().IsArray)
            {
                if (depth + 1 > maxDepth)
                {
                    list.Add("[...]");
                    continue;
                }

                var arr = ((item as Array) ?? Array.Empty<object>()).OfType<object?>().ToArray();
                var result = ToString(arr, depth + 1, maxDepth);

                list.Add($"[{string.Join(", ", result)}]");

                continue;
            }

            list.Add(item.ToString() ?? "null");
        }

        return string.Join(", ", list);
    }
}
