using System.Collections.Generic;
using System.Linq;
using System.Text;
using Confirma.Formatters;

namespace Confirma.Helpers;

public static class CollectionHelper
{
    public static string ToString<T>(
        IEnumerable<T> collection,
        uint depth = 0,
        uint maxDepth = 1,
        bool addBrackets = true,
        bool addTypeHint = true
    )
    {
        string typeName = addTypeHint ? typeof(T).Name : string.Empty;

        if (depth > maxDepth || !collection.Any())
        {
            return addBrackets ? typeName + "[]" : string.Empty;
        }

        StringBuilder sb = new();

        foreach (T item in collection)
        {
            if (item is null)
            {
                _ = sb.Append("null");
            }
            else if (item is IEnumerable<object> e && item is not string)
            {
                if (depth + 1 > maxDepth)
                {
                    _ = sb.Append(typeName + "[...]");
                }
                else
                {
                    string result = ToString(e, depth + 1, maxDepth, false, false);
                    _ = sb.Append(typeName + $"[{string.Join(", ", result)}]");
                }
            }
            else
            {
                _ = sb.Append(new AutomaticFormatter().Format(item));
            }

            _ = sb.Append(", ");
        }

        // Remove the trailing comma and space
        if (sb.Length > 2)
        {
            sb.Length -= 2;
        }

        string text = sb.ToString();

        return addBrackets ? typeName + $"[{text}]" : text;
    }
}
