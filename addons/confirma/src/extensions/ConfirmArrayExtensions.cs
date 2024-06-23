using System;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmArrayExtensions
{
	public static void ConfirmSize<T>(this T[] array, int expectedSize, string? message = null)
	{
		if (array.Length == expectedSize) return;

		throw new ConfirmAssertException(message ?? $"Array size is {array.Length} but expected {expectedSize}.");
	}

	public static void ConfirmEmpty<T>(this T[] array, string? message = null)
	{
		if (array.Length == 0) return;

		throw new ConfirmAssertException(message ?? "Expected array to be empty.");
	}

	public static void ConfirmNotEmpty<T>(this T[] array, string? message = null)
	{
		if (array.Length > 0) return;

		throw new ConfirmAssertException(message ?? "Expected array to not be empty.");
	}

	public static void ConfirmContains<T>(this T[] array, T expected, string? message = null)
	{
		if (Array.IndexOf(array, expected) != -1) return;

		throw new ConfirmAssertException(message ?? $"Expected array to contain '{expected}'.");
	}

	public static void ConfirmNotContains<T>(this T[] array, T expected, string? message = null)
	{
		if (Array.IndexOf(array, expected) == -1) return;

		throw new ConfirmAssertException(message ?? $"Expected array to not contain '{expected}'.");
	}
}
