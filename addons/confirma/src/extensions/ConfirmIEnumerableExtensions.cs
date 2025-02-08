using System;
using System.Collections.Generic;
using System.Linq;
using Confirma.Exceptions;
using Confirma.Formatters;

namespace Confirma.Extensions;

public static class ConfirmIEnumerableExtensions
{
    public static IEnumerable<T> ConfirmEmpty<T>(
        this IEnumerable<T> actual,
        string? message = null
    )
    {
        if (!actual.Any())
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected empty enumerable, but found {1} elements.",
            nameof(ConfirmEmpty),
            new NumericFormatter(),
            actual.Count(),
            null,
            message
        );
    }

    public static IEnumerable<T> ConfirmNotEmpty<T>(
        this IEnumerable<T> actual,
        string? message = null
    )
    {
        if (actual.Any())
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected non-empty enumerable.",
            nameof(ConfirmNotEmpty),
            null,
            null,
            null,
            message
        );
    }

    public static IEnumerable<T> ConfirmCount<T>(
        this IEnumerable<T> actual,
        int expected,
        string? message = null
    )
    {
        if (actual.Count() == expected)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected enumerable with {1} elements, but found {2}.",
            nameof(ConfirmCount),
            new NumericFormatter(),
            expected,
            actual.Count(),
            message
        );
    }

    public static IEnumerable<T> ConfirmCountGreaterThan<T>(
        this IEnumerable<T> actual,
        int expected,
        string? message = null
    )
    {
        if (actual.Count() > expected)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected enumerable with more than {1} elements, but found {2}.",
            nameof(ConfirmCountGreaterThan),
            new NumericFormatter(),
            expected,
            actual.Count(),
            message
        );
    }

    public static IEnumerable<T> ConfirmCountLessThan<T>(
        this IEnumerable<T> actual,
        int expected,
        string? message = null
    )
    {
        if (actual.Count() < expected)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected enumerable with fewer than {1} elements, but found {2}.",
            nameof(ConfirmCountLessThan),
            new NumericFormatter(),
            expected,
            actual.Count(),
            message
        );
    }

    public static IEnumerable<T> ConfirmCountGreaterThanOrEqual<T>(
        this IEnumerable<T> actual,
        int expected,
        string? message = null
    )
    {
        if (actual.Count() >= expected)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected enumerable with at least {1} elements, but found {2}.",
            nameof(ConfirmCountGreaterThanOrEqual),
            new NumericFormatter(),
            expected,
            actual.Count(),
            message
        );
    }

    public static IEnumerable<T> ConfirmCountLessThanOrEqual<T>(
        this IEnumerable<T> actual,
        int expected,
        string? message = null
    )
    {
        if (actual.Count() <= expected)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected enumerable with at most {1} elements, but found {2}.",
            nameof(ConfirmCountLessThanOrEqual),
            new NumericFormatter(),
            expected,
            actual.Count(),
            message
        );
    }

    public static IEnumerable<T> ConfirmContains<T>(
        this IEnumerable<T> actual,
        T expected,
        string? message = null
    )
    {
        if (actual.Contains(expected))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected enumerable to contain {1}.",
            nameof(ConfirmContains),
            new AutomaticFormatter(),
            expected,
            null,
            message
        );
    }

    public static IEnumerable<T> ConfirmNotContains<T>(
        this IEnumerable<T> actual,
        T expected,
        string? message = null
    )
    {
        if (!actual.Contains(expected))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected enumerable to not contain {1}.",
            nameof(ConfirmNotContains),
            new AutomaticFormatter(),
            expected,
            null,
            message
        );
    }

    public static IEnumerable<T> ConfirmAllMatch<T>(
        this IEnumerable<T> actual,
        Func<T, bool> predicate,
        string? message = null
    )
    {
        if (actual.All(predicate))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected elements to match the predicate.",
            nameof(ConfirmAllMatch),
            null,
            null,
            null,
            message
        );
    }

    public static IEnumerable<T> ConfirmAnyMatch<T>(
        this IEnumerable<T> actual,
        Func<T, bool> predicate,
        string? message = null
    )
    {
        if (actual.Any(predicate))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected at least one element to match the predicate.",
            nameof(ConfirmAnyMatch),
            null,
            null,
            null,
            message
        );
    }

    public static IEnumerable<T> ConfirmNoneMatch<T>(
        this IEnumerable<T> actual,
        Func<T, bool> predicate,
        string? message = null
    )
    {
        if (actual.All(x => !predicate(x)))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected no elements to match the predicate.",
            nameof(ConfirmNoneMatch),
            null,
            null,
            null,
            message
        );
    }

    public static IEnumerable<T> ConfirmElementsAreUnique<T>(
        this IEnumerable<T> actual,
        string? message = null
    )
    {
        if (actual.Distinct().Count() == actual.Count())
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected elements to be unique.",
            nameof(ConfirmElementsAreUnique),
            null,
            null,
            null,
            message
        );
    }

    public static IEnumerable<T> ConfirmElementsAreDistinct<T>(
        this IEnumerable<T> actual,
        IEnumerable<T> expected,
        string? message = null
    )
    {
        if (actual.Distinct().Count() == expected.Distinct().Count())
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected elements to be distinct from the expected set.",
            nameof(ConfirmElementsAreDistinct),
            null,
            null,
            null,
            message
        );
    }

    #region ConfirmElementsAreOrdered
    public static IEnumerable<T> ConfirmElementsAreOrdered<T>(
        this IEnumerable<T> actual,
        string? message = null
    )
    {
        if (actual.Order().SequenceEqual(actual))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected elements to be in order.",
            nameof(ConfirmElementsAreOrdered),
            null,
            null,
            null,
            message
        );
    }

    public static IEnumerable<T> ConfirmElementsAreOrdered<T>(
        this IEnumerable<T> actual,
        IComparer<T> comparer,
        string? message = null
    )
    {
        if (actual.Order(comparer).SequenceEqual(actual))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected elements to be in order.",
            nameof(ConfirmElementsAreOrdered),
            null,
            null,
            null,
            message
        );
    }
    #endregion ConfirmElementsAreOrdered

    #region ConfirmElementsAreNotOrdered
    public static IEnumerable<T> ConfirmElementsAreNotOrdered<T>(
        this IEnumerable<T> actual,
        string? message = null
    )
    {
        if (!actual.Order().SequenceEqual(actual))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected elements not to be in order.",
            nameof(ConfirmElementsAreNotOrdered),
            null,
            null,
            null,
            message
        );
    }

    public static IEnumerable<T> ConfirmElementsAreNotOrdered<T>(
        this IEnumerable<T> actual,
        IComparer<T> comparer,
        string? message = null
    )
    {
        if (!actual.Order(comparer).SequenceEqual(actual))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected elements not to be in order.",
            nameof(ConfirmElementsAreNotOrdered),
            null,
            null,
            null,
            message
        );
    }
    #endregion ConfirmElementsAreNotOrdered

    public static IEnumerable<T> ConfirmElementsAreInRange<T>(
        this IEnumerable<T> actual,
        T from,
        T to,
        string? message = null
    )
    where T : IComparable<T>
    {
        if (actual.All(x => x.CompareTo(from) >= 0 && x.CompareTo(to) <= 0))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected elements to be within the range [{1}, {2}].",
            nameof(ConfirmElementsAreInRange),
            new NumericFormatter(),
            from,
            to,
            message
        );
    }

    public static IEnumerable<T> ConfirmElementsAreEquivalent<T>(
        this IEnumerable<T> actual,
        IEnumerable<T> expected,
        string? message = null
    )
    {
        if (actual.Order().SequenceEqual(expected.Order()))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected elements to be equivalent to the expected set.",
            nameof(ConfirmElementsAreEquivalent),
            null,
            null,
            null,
            message
        );
    }
}
