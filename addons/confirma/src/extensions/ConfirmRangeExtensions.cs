using System;
using Confirma.Exceptions;
using Confirma.Formatters;

namespace Confirma.Extensions;

public static class ConfirmRangeExtensions
{
    public static T ConfirmInRange<T>(
        this T actual,
        T min,
        T max,
        string? message = null
    )
    where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        if (actual.CompareTo(min) >= 0 && actual.CompareTo(max) <= 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            $"Expected {new NumericFormatter().Format(actual)} "
            + "to be within the range [{1}, {2}].",
            nameof(ConfirmInRange),
            new NumericFormatter(),
            min,
            max,
            message
        );
    }

    public static T ConfirmNotInRange<T>(
        this T actual,
        T min,
        T max,
        string? message = null
    )
    where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        if (actual.CompareTo(min) < 0 || actual.CompareTo(max) > 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            $"Expected {new NumericFormatter().Format(actual)} "
            + "to be outside the range [{1}, {2}].",
            nameof(ConfirmNotInRange),
            new NumericFormatter(),
            min,
            max,
            message
        );
    }

    public static T ConfirmGreaterThan<T>(
        this T actual,
        T value,
        string? message = null
    )
    where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        if (actual.CompareTo(value) > 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected {1} to be greater than {2}.",
            nameof(ConfirmGreaterThan),
            new NumericFormatter(),
            actual,
            value,
            message
        );
    }

    public static T ConfirmGreaterThanOrEqual<T>(
        this T actual,
        T value,
        string? message = null
    )
    where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        if (actual.CompareTo(value) >= 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected {1} to be greater than or equal {2}.",
            nameof(ConfirmGreaterThanOrEqual),
            new NumericFormatter(),
            actual,
            value,
            message
        );
    }

    public static T ConfirmLessThan<T>(
        this T actual,
        T value,
        string? message = null
    )
    where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        if (actual.CompareTo(value) < 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected {1} to be less than {2}.",
            nameof(ConfirmLessThan),
            new NumericFormatter(),
            actual,
            value,
            message
        );
    }

    public static T ConfirmLessThanOrEqual<T>(
        this T actual,
        T value,
        string? message = null
    )
    where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        if (actual.CompareTo(value) <= 0)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected {1} to be less than or equal {2}.",
            nameof(ConfirmLessThanOrEqual),
            new NumericFormatter(),
            actual,
            value,
            message
        );
    }
}
