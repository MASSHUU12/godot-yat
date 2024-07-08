using System;
using System.Collections.Generic;
using System.Linq;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmIEnumerableExtensions
{
    public static IEnumerable<T> ConfirmEmpty<T>(this IEnumerable<T> actual, string? message = null)
    {
        if (!actual.Any()) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected empty enumerable, but it contained {actual.Count()} elements."
        );
    }

    public static IEnumerable<T> ConfirmNotEmpty<T>(this IEnumerable<T> actual, string? message = null)
    {
        if (actual.Any()) return actual;

        throw new ConfirmAssertException(
            message ??
            "Expected non-empty enumerable, but it was empty."
        );
    }

    public static IEnumerable<T> ConfirmCount<T>(this IEnumerable<T> actual, int expected, string? message = null)
    {
        if (actual.Count() == expected) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected {expected} elements, but found {actual.Count()}."
        );
    }

    public static IEnumerable<T> ConfirmCountGreaterThan<T>(this IEnumerable<T> actual, int expected, string? message = null)
    {
        if (actual.Count() > expected) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected more than {expected} elements, but found {actual.Count()}."
        );
    }

    public static IEnumerable<T> ConfirmCountLessThan<T>(this IEnumerable<T> actual, int expected, string? message = null)
    {
        if (actual.Count() < expected) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected fewer than {expected} elements, but found {actual.Count()}."
        );
    }

    public static IEnumerable<T> ConfirmCountGreaterThanOrEqual<T>(this IEnumerable<T> actual, int expected, string? message = null)
    {
        if (actual.Count() >= expected) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected at least {expected} elements, but found {actual.Count()}."
        );
    }

    public static IEnumerable<T> ConfirmCountLessThanOrEqual<T>(this IEnumerable<T> actual, int expected, string? message = null)
    {
        if (actual.Count() <= expected) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected at most {expected} elements, but found {actual.Count()}."
        );
    }

    public static IEnumerable<T> ConfirmContains<T>(this IEnumerable<T> actual, T expected, string? message = null)
    {
        if (actual.Contains(expected)) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected '{expected}' to be in the enumerable, but it was not."
        );
    }

    public static IEnumerable<T> ConfirmNotContains<T>(this IEnumerable<T> actual, T expected, string? message = null)
    {
        if (!actual.Contains(expected)) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected '{expected}' not to be in the enumerable, but it was."
        );
    }

    public static IEnumerable<T> ConfirmAllMatch<T>(this IEnumerable<T> actual, Func<T, bool> predicate, string? message = null)
    {
        if (actual.All(predicate)) return actual;

        throw new ConfirmAssertException(
            message ??
            "Expected all elements to match the predicate, but some did not."
        );
    }

    public static IEnumerable<T> ConfirmAnyMatch<T>(this IEnumerable<T> actual, Func<T, bool> predicate, string? message = null)
    {
        if (actual.Any(predicate)) return actual;

        throw new ConfirmAssertException(
            message ??
            "Expected at least one element to match the predicate, but none did."
        );
    }

    public static IEnumerable<T> ConfirmNoneMatch<T>(this IEnumerable<T> actual, Func<T, bool> predicate, string? message = null)
    {
        if (actual.All(x => !predicate(x))) return actual;

        throw new ConfirmAssertException(
            message ??
            "Expected no elements to match the predicate, but some did."
        );
    }

    public static IEnumerable<T> ConfirmElementsAreUnique<T>(this IEnumerable<T> actual, string? message = null)
    {
        if (actual.Distinct().Count() == actual.Count()) return actual;

        throw new ConfirmAssertException(
            message ??
            "Expected all elements to be unique, but duplicates were found."
        );
    }

    public static IEnumerable<T> ConfirmElementsAreDistinct<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string? message = null)
    {
        if (actual.Distinct().Count() == expected.Distinct().Count()) return actual;

        throw new ConfirmAssertException(
            message ??
            "Expected elements to be distinct from the expected set, but some were not."
        );
    }

    public static IEnumerable<T> ConfirmElementsAreOrdered<T>(this IEnumerable<T> actual, string? message = null)
    {
        if (actual.OrderBy(x => x).SequenceEqual(actual)) return actual;

        throw new ConfirmAssertException(
            message ??
            "Expected elements to be in order, but they were not."
        );
    }

    public static IEnumerable<T> ConfirmElementsAreInRange<T>(this IEnumerable<T> actual, T from, T to, string? message = null)
    where T : IComparable<T>
    {
        if (actual.All(x => x.CompareTo(from) >= 0 && x.CompareTo(to) <= 0)) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected all elements to be within the range [{from}, {to}], but some were not."
        );
    }

    public static IEnumerable<T> ConfirmElementsAreEquivalent<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string? message = null)
    {
        if (actual.OrderBy(x => x).SequenceEqual(expected.OrderBy(x => x))) return actual;

        throw new ConfirmAssertException(
            message ??
            "Expected elements to be equivalent to the expected set, but they were not."
        );
    }
}
