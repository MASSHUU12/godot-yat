using System;

namespace Confirma;

public static class ConfirmRangeExtension
{
	public static void ConfirmInRange<T>(this T actual, T min, T max, string? message = null)
	where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
	{
		if (actual.CompareTo(min) < 0 || actual.CompareTo(max) > 0)
			throw new ConfirmAssertException(message ?? $"Expected object to be in range [{min}, {max}].");
	}

	public static void ConfirmNotInRange<T>(this T actual, T min, T max, string? message = null)
	where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
	{
		if (actual.CompareTo(min) >= 0 && actual.CompareTo(max) <= 0)
			throw new ConfirmAssertException(message ?? $"Expected object to be not in range [{min}, {max}].");
	}

	public static void ConfirmGraterThan<T>(this T actual, T value, string? message = null)
	where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
	{
		if (actual.CompareTo(value) <= 0)
			throw new ConfirmAssertException(message ?? $"Expected object to be grater than {value}.");
	}

	public static void ConfirmGraterThanOrEqual<T>(this T actual, T value, string? message = null)
	where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
	{
		if (actual.CompareTo(value) < 0)
			throw new ConfirmAssertException(message ?? $"Expected object to be grater than or equal to {value}.");
	}

	public static void ConfirmLessThan<T>(this T actual, T value, string? message = null)
	where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
	{
		if (actual.CompareTo(value) >= 0)
			throw new ConfirmAssertException(message ?? $"Expected object to be less than {value}.");
	}

	public static void ConfirmLessThanOrEqual<T>(this T actual, T value, string? message = null)
	where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
	{
		if (actual.CompareTo(value) > 0)
			throw new ConfirmAssertException(message ?? $"Expected object to be less than or equal to {value}.");
	}
}
