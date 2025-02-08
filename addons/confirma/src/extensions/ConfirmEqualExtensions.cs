using System;
using System.Linq;
using Confirma.Exceptions;
using Confirma.Formatters;

namespace Confirma.Extensions;

public static class ConfirmEqualExtensions
{
    #region ConfirmEqual
    public static T? ConfirmEqual<T>(
        this T? actual,
        T? expected,
        string? message = null
    )
    {
        if (actual is Array actualArray && expected is Array expectedArray)
        {
            return (T)(actualArray.ConfirmEqual(expectedArray, message) as object);
        }

        if (!(!actual?.Equals(expected) ?? false))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected {1}, but got {2}.",
            nameof(ConfirmEqual),
            new AutomaticFormatter(),
            expected,
            actual,
            message,
            formatNulls: 3
        );
    }

    public static Array ConfirmEqual(
        this Array actual,
        Array expected,
        string? message = null
    )
    {
        if (actual.Length != expected.Length)
        {
            throw new ConfirmAssertException(
                "Expected array of length {1}, but got array of length {2}.",
                nameof(ConfirmEqual),
                new NumericFormatter(),
                expected.Length,
                actual.Length,
                message
            );
        }

        for (int i = 0; i < actual.Length; i++)
        {
            object? actualValue = actual.GetValue(i);
            object? expectedValue = expected.GetValue(i);

            if (!Equals(actualValue, expectedValue))
            {
                throw new ConfirmAssertException(
                    $"Arrays differ at index {i}. "
                    + "Expected {1}, but got {2}.",
                    nameof(ConfirmEqual),
                    new AutomaticFormatter(),
                    expectedValue,
                    actualValue,
                    message,
                    formatNulls: 3
                );
            }
        }

        return actual;
    }

    public static T?[] ConfirmEqual<T>(
        this T?[] actual,
        T?[] expected,
        string? message = null
    )
    {
        if (actual.SequenceEqual(expected))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected {1}, but got {2}.",
            nameof(ConfirmEqual),
            new AutomaticFormatter(),
            expected,
            actual,
            message,
            formatNulls: 3
        );
    }
    #endregion ConfirmEqual

    #region ConfirmNotEqual
    public static T? ConfirmNotEqual<T>(
        this T? actual,
        T? expected,
        string? message = null
    )
    {
        if (actual is Array actualArray && expected is Array expectedArray)
        {
            return (T)(actualArray.ConfirmNotEqual(expectedArray, message) as object);
        }

        if (!(actual?.Equals(expected) ?? false))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected not {1}.",
            nameof(ConfirmNotEqual),
            new AutomaticFormatter(),
            expected,
            null,
            message,
            formatNulls: 1
        );
    }

    public static Array ConfirmNotEqual(
        this Array actual,
        Array expected,
        string? message = null
    )
    {
        bool isDifferent = false;

        for (int i = 0; i < actual.Length; i++)
        {
            object? actualValue = actual.GetValue(i);
            object? expectedValue = expected.GetValue(i);

            if (!Equals(actualValue, expectedValue))
            {
                isDifferent = true;
            }
        }

        return isDifferent
            ? actual
            : throw new ConfirmAssertException(
                "Expected not {1}.",
                nameof(ConfirmNotEqual),
                new AutomaticFormatter(),
                expected,
                null,
                message,
                formatNulls: 1
            );
    }

    public static T?[] ConfirmNotEqual<T>(
        this T?[] actual,
        T?[] expected,
        string? message = null
    )
    {
        if (!actual.SequenceEqual(expected))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected not {1}.",
            nameof(ConfirmNotEqual),
            new AutomaticFormatter(),
            expected,
            null,
            message,
            formatNulls: 1
        );
    }
    #endregion ConfirmNotEqual

    #region ConfirmDefaultValue
    public static T? ConfirmDefaultValue<T>(
        this T? actual,
        string? message = null
    )
    {
        if (Equals(actual, default(T?)))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "{1} is not the default value of the " + $"{typeof(T?).Name}.",
            nameof(ConfirmDefaultValue),
            new AutomaticFormatter(),
            actual,
            null,
            message,
            formatNulls: 1
        );
    }

    public static T? ConfirmNotDefaultValue<T>(
        this T? actual,
        string? message = null
    )
    {
        if (!Equals(actual, default(T?)))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "{1} is the default value of the " + $"{typeof(T?).Name}.",
            nameof(ConfirmNotDefaultValue),
            new AutomaticFormatter(),
            actual,
            null,
            message,
            formatNulls: 1
        );
    }
    #endregion ConfirmDefaultValue
}
