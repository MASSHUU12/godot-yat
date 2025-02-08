using System;
using System.Numerics;
using Confirma.Exceptions;
using Confirma.Formatters;
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
            "Expected {1} to be positive.",
            nameof(ConfirmIsPositive),
            new NumericFormatter(),
            actual,
            null,
            message
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
            "Expected {1} to be not positive.",
            nameof(ConfirmIsNotPositive),
            new NumericFormatter(),
            actual,
            null,
            message
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
            "Expected {1} to be negative.",
            nameof(ConfirmIsNegative),
            new NumericFormatter(),
            actual,
            null,
            message
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
            "Expected {1} to be not negative.",
            nameof(ConfirmIsNotNegative),
            new NumericFormatter(),
            actual,
            null,
            message
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
        object? zero = (T)Convert.ChangeType(0, typeof(T), InvariantCulture);

        return sign switch
        {
            true when actual.CompareTo(zero) < 0 => actual,
            false when actual.CompareTo(zero) > 0 => actual,
            _ => throw new ConfirmAssertException(
                "Expected {1} to have a "
                + $"{(sign ? "negative" : "positive")} sign.",
                nameof(ConfirmSign),
                new NumericFormatter(),
                actual,
                null,
                message
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
            "Expected {1} to be zero.",
            nameof(ConfirmIsZero),
            new NumericFormatter(),
            actual,
            null,
            message
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
            "Expected {1} to be not zero.",
            nameof(ConfirmIsNotZero),
            new NumericFormatter(),
            actual,
            null,
            message
        );
    }
    #endregion ConfirmIsZero

    #region ConfirmIsNaN
    public static double ConfirmIsNaN(
        this double actual,
        string? message = null
    )
    {
        if (double.IsNaN(actual))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected {1} to be NaN.",
            nameof(ConfirmIsNaN),
            new NumericFormatter(),
            actual,
            null,
            message
        );
    }

    public static double ConfirmIsNotNaN(
        this double actual,
        string? message = null
    )
    {
        if (!double.IsNaN(actual))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected value not to be NaN.",
            nameof(ConfirmIsNotNaN),
            null,
            null,
            null,
            message
        );
    }
    #endregion ConfirmIsNaN

    public static T ConfirmIsOdd<T>(this T actual, string? message = null)
        where T : INumber<T>
    {
        if ((Convert.ToInt64(actual, InvariantCulture) & 1) != 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected {1} to be odd.",
            nameof(ConfirmIsOdd),
            new NumericFormatter(),
            actual,
            null,
            message
        );
    }

    public static T ConfirmIsEven<T>(this T actual, string? message = null)
        where T : INumber<T>
    {
        if ((Convert.ToInt64(actual, InvariantCulture) & 1) == 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected {1} to be even.",
            nameof(ConfirmIsEven),
            new NumericFormatter(),
            actual,
            null,
            message
        );
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
            "Expected {1} to be close to {2}.",
            nameof(ConfirmCloseTo),
            new NumericFormatter(),
            actual,
            expected,
            message
        );
    }
}
