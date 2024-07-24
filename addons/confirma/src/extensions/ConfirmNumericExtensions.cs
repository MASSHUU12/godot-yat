using System;
using System.Numerics;
using Confirma.Exceptions;
using static System.Globalization.CultureInfo;

namespace Confirma.Extensions;

public static class ConfirmNumericExtensions
{
    #region ConfirmIsPositive

    /// <remarks>
    /// Zero is not considered positive.
    /// </remarks>
    public static T ConfirmIsPositive<T>(this T actual, string? message = null)
    where T : INumber<T>
    {
        if (actual.CompareTo((T)Convert.ChangeType(0, typeof(T), InvariantCulture)) > 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected {actual} to be positive."
        );
    }

    public static T ConfirmIsNotPositive<T>(this T actual, string? message = null)
        where T : INumber<T>
    {
        if (actual.CompareTo((T)Convert.ChangeType(0, typeof(T), InvariantCulture)) <= 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected {actual} to not be positive."
        );
    }

    #endregion ConfirmIsPositive

    #region ConfirmIsNegative

    /// <remarks>
    /// Zero is not considered negative.
    /// </remarks>
    public static T ConfirmIsNegative<T>(this T actual, string? message = null)
        where T : INumber<T>
    {
        if (actual.CompareTo((T)Convert.ChangeType(0, typeof(T), InvariantCulture)) < 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected {actual} to be negative."
        );
    }

    public static T ConfirmIsNotNegative<T>(this T actual, string? message = null)
        where T : INumber<T>
    {
        if (actual.CompareTo((T)Convert.ChangeType(0, typeof(T), InvariantCulture)) >= 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected {actual} to not be negative."
        );
    }

    #endregion ConfirmIsNegative

    #region ConfirmSign

    /// <remarks>
    /// Zero is not considered signed or unsigned.
    /// </remarks>
    public static T ConfirmSign<T>(this T actual, bool sign, string? message = null)
        where T : INumber<T>
    {
        return sign switch
        {
            true when actual.CompareTo((T)Convert.ChangeType(0, typeof(T))) < 0 => actual,
            false when actual.CompareTo((T)Convert.ChangeType(0, typeof(T))) > 0 => actual,
            _ => throw new ConfirmAssertException(
                message
                ?? $"Expected {actual} to have a {(sign ? "negative" : "positive")} sign."
            )
        };
    }

    #endregion ConfirmSign

    #region ConfirmIsZero

    public static T ConfirmIsZero<T>(this T actual, string? message = null)
        where T : INumber<T>
    {
        if (actual.CompareTo((T)Convert.ChangeType(0, typeof(T), InvariantCulture)) == 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected {actual} to be zero."
        );
    }

    public static T ConfirmIsNotZero<T>(this T actual, string? message = null)
        where T : INumber<T>
    {
        if (actual.CompareTo((T)Convert.ChangeType(0, typeof(T), InvariantCulture)) != 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected {actual} to not be zero."
        );
    }

    #endregion ConfirmIsZero

    public static T ConfirmIsOdd<T>(this T actual, string? message = null)
        where T : INumber<T>
    {
        if ((Convert.ToInt64(actual, InvariantCulture) & 1) != 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(message ?? $"Expected {actual} to be odd.");
    }

    public static T ConfirmIsEven<T>(this T actual, string? message = null)
        where T : INumber<T>
    {
        if ((Convert.ToInt64(actual, InvariantCulture) & 1) == 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(message ?? $"Expected {actual} to be even.");
    }

    public static T ConfirmCloseTo<T>(
        this T actual,
        T expected,
        T tolerance,
        string? message = null
    )
        where T : INumber<T>
    {
        T diff = actual - expected;
        T abs = diff < (T)Convert.ChangeType(0, typeof(T), InvariantCulture) ? -diff : diff;

        if (abs <= tolerance)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"{actual} is not close to {expected} within tolerance {tolerance}."
        );
    }
}
