using System;
using System.Collections.Generic;
using System.Linq;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmIEnumerableExtensions
{
	public static void ConfirmEmpty<T>(this IEnumerable<T> actual, string? message = null)
	{
		if (!actual.Any()) return;

		throw new ConfirmAssertException(message ?? "Expected empty but was not.");
	}

	public static void ConfirmNotEmpty<T>(this IEnumerable<T> actual, string? message = null)
	{
		if (actual.Any()) return;

		throw new ConfirmAssertException(message ?? "Expected not empty but was.");
	}

	public static void ConfirmCount<T>(this IEnumerable<T> actual, int expected, string? message = null)
	{
		if (actual.Count() == expected) return;

		throw new ConfirmAssertException(message ?? $"Expected count of {expected} but was {actual.Count()}.");
	}

	public static void ConfirmCountGreaterThan<T>(this IEnumerable<T> actual, int expected, string? message = null)
	{
		if (actual.Count() > expected) return;

		throw new ConfirmAssertException(message ?? $"Expected count to be greater than {expected} but was {actual.Count()}.");
	}

	public static void ConfirmCountLessThan<T>(this IEnumerable<T> actual, int expected, string? message = null)
	{
		if (actual.Count() < expected) return;

		throw new ConfirmAssertException(message ?? $"Expected count to be less than {expected} but was {actual.Count()}.");
	}

	public static void ConfirmCountGreaterThanOrEqual<T>(this IEnumerable<T> actual, int expected, string? message = null)
	{
		if (actual.Count() >= expected) return;

		throw new ConfirmAssertException(message ?? $"Expected count to be greater than or equal to {expected} but was {actual.Count()}.");
	}

	public static void ConfirmCountLessThanOrEqual<T>(this IEnumerable<T> actual, int expected, string? message = null)
	{
		if (actual.Count() <= expected) return;

		throw new ConfirmAssertException(message ?? $"Expected count to be less than or equal to {expected} but was {actual.Count()}.");
	}

	public static void ConfirmContains<T>(this IEnumerable<T> actual, T expected, string? message = null)
	{
		if (actual.Contains(expected)) return;

		throw new ConfirmAssertException(message ?? $"Expected to contain '{expected}' but did not.");
	}

	public static void ConfirmNotContains<T>(this IEnumerable<T> actual, T expected, string? message = null)
	{
		if (!actual.Contains(expected)) return;

		throw new ConfirmAssertException(message ?? $"Expected not to contain '{expected}' but did.");
	}

	public static void ConfirmAllMatch<T>(this IEnumerable<T> actual, Func<T, bool> predicate, string? message = null)
	{
		if (actual.All(predicate)) return;

		throw new ConfirmAssertException(message ?? "Expected all to match but did not.");
	}

	public static void ConfirmAnyMatch<T>(this IEnumerable<T> actual, Func<T, bool> predicate, string? message = null)
	{
		if (actual.Any(predicate)) return;

		throw new ConfirmAssertException(message ?? "Expected any to match but did not.");
	}

	public static void ConfirmNoneMatch<T>(this IEnumerable<T> actual, Func<T, bool> predicate, string? message = null)
	{
		if (actual.All(x => !predicate(x))) return;

		throw new ConfirmAssertException(message ?? "Expected none to match but did.");
	}

	public static void ConfirmElementsAreUnique<T>(this IEnumerable<T> actual, string? message = null)
	{
		if (actual.Distinct().Count() == actual.Count()) return;

		throw new ConfirmAssertException(message ?? "Expected elements to be unique but were not.");
	}

	public static void ConfirmElementsAreDistinct<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string? message = null)
	{
		if (actual.Distinct().Count() == expected.Distinct().Count()) return;

		throw new ConfirmAssertException(message ?? "Expected elements to be distinct but were not.");
	}

	public static void ConfirmElementsAreOrdered<T>(this IEnumerable<T> actual, string? message = null)
	{
		if (actual.OrderBy(x => x).SequenceEqual(actual)) return;

		throw new ConfirmAssertException(message ?? "Expected elements to be ordered but were not.");
	}

	public static void ConfirmElementsAreInRange<T>(this IEnumerable<T> actual, T from, T to, string? message = null)
		where T : IComparable<T>
	{
		if (actual.All(x => x.CompareTo(from) >= 0 && x.CompareTo(to) <= 0)) return;

		throw new ConfirmAssertException(message ?? $"Expected elements to be in range [{from}, {to}] but were not.");
	}

	public static void ConfirmElementsAreEquivalent<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string? message = null)
	{
		if (actual.OrderBy(x => x).SequenceEqual(expected.OrderBy(x => x))) return;

		throw new ConfirmAssertException(message ?? "Expected elements to be equivalent but were not.");
	}
}
