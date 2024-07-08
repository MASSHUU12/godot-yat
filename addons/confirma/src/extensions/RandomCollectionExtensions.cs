using System;
using System.Collections.Generic;
using System.Linq;

namespace Confirma.Extensions;

public static class RandomCollectionExtensions
{
    public static T NextElement<T>(this Random rg, IEnumerable<T> elements)
    {
        if (!elements.Any())
        {
            throw new InvalidOperationException(
                "Cannot get a random element from an empty collection."
            );
        }

        return elements.ElementAt(rg.Next(0, elements.Count()));
    }

    public static IEnumerable<T> NextElements<T>(
        this Random rg,
        int numberOfElements,
        IEnumerable<T> elements
    )
    {
        if (numberOfElements == 0)
        {
            throw new InvalidOperationException(
                "Cannot take zero random elements from a collection."
            );
        }

        if (numberOfElements > elements.Count())
        {
            throw new InvalidOperationException(
                "Cannot to take more random elements than there are in the collection."
            );
        }

        var list = new List<T>();
        for (int i = 0; i < numberOfElements; i++)
        {
            list.Add(rg.NextElement(elements));
        }

        return list;
    }

    public static IEnumerable<T> NextShuffle<T>(this Random rg, IEnumerable<T> elements)
    {
        if (!elements.Any())
        {
            throw new InvalidOperationException(
                "Cannot shuffle an empty collection."
            );
        }

        List<T> shuffled = new(elements);
        int n = shuffled.Count;

        while (n > 1)
        {
            n--;
            int k = rg.Next(n + 1);
            (shuffled[n], shuffled[k]) = (shuffled[k], shuffled[n]);
        }

        return shuffled;
    }
}
