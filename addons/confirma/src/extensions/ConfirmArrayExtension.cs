using System;

namespace Confirma;

public static class ConfirmArrayExtension
{
	public static void ConfirmSize<T>(this T[] array, int expectedSize)
	{
		if (array.Length == expectedSize) return;

		throw new ConfirmAssertException($"Array size is {array.Length} but expected {expectedSize}");
	}

	public static void ConfirmEmpty<T>(this T[] array)
	{
		if (array.Length == 0) return;

		throw new ConfirmAssertException("Array is not empty");
	}

	public static void ConfirmNotEmpty<T>(this T[] array)
	{
		if (array.Length > 0) return;

		throw new ConfirmAssertException("Array is empty");
	}

	public static void ConfirmContains<T>(this T[] array, T expected)
	{
		if (Array.IndexOf(array, expected) != -1) return;

		throw new ConfirmAssertException($"Array does not contain {expected}");
	}

	public static void ConfirmNotContains<T>(this T[] array, T expected)
	{
		if (Array.IndexOf(array, expected) == -1) return;

		throw new ConfirmAssertException($"Array contains {expected}");
	}
}
