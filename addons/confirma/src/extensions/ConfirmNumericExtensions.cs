using System;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmNumericExtensions
{
	#region ConfirmIsPositive
	/// <remarks>
	/// This extension method can be used to validate that a value is positive.
	/// <b>Zero is not considered positive.</b>
	/// </remarks>
	public static void ConfirmIsPositive<T>(this T actual, string? message = null)
	where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
	{
		if (actual.CompareTo((T)Convert.ChangeType(0, typeof(T))) > 0) return;
		throw new ConfirmAssertException(message ?? "Expected object to be positive.");
	}

	public static void ConfirmIsNotPositive<T>(this T actual, string? message = null)
	where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
	{
		if (actual.CompareTo((T)Convert.ChangeType(0, typeof(T))) <= 0) return;
		throw new ConfirmAssertException(message ?? "Expected object to not be positive.");
	}
	#endregion

	#region ConfirmIsNegative
	/// <remarks>
	/// This extension method can be used to validate that a value is negative.
	/// <b>Zero is not considered negative.</b>
	/// </remarks>
	public static void ConfirmIsNegative<T>(this T actual, string? message = null)
	where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
	{
		if (actual.CompareTo((T)Convert.ChangeType(0, typeof(T))) < 0) return;
		throw new ConfirmAssertException(message ?? "Expected object to be negative.");
	}

	public static void ConfirmIsNotNegative<T>(this T actual, string? message = null)
	where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
	{
		if (actual.CompareTo((T)Convert.ChangeType(0, typeof(T))) >= 0) return;
		throw new ConfirmAssertException(message ?? "Expected object to not be negative.");
	}
	#endregion

	#region ConfirmIsZero
	public static void ConfirmIsZero<T>(this T actual, string? message = null)
	where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
	{
		if (actual.CompareTo((T)Convert.ChangeType(0, typeof(T))) == 0) return;
		throw new ConfirmAssertException(message ?? "Expected object to be zero.");
	}

	public static void ConfirmIsNotZero<T>(this T actual, string? message = null)
	where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
	{
		if (actual.CompareTo((T)Convert.ChangeType(0, typeof(T))) != 0) return;
		throw new ConfirmAssertException(message ?? "Expected object to not be zero.");
	}
	#endregion
}
