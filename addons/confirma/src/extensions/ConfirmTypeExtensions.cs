using System;
using Confirma.Exceptions;
using Confirma.Formatters;

namespace Confirma.Extensions;

public static class ConfirmTypeExtensions
{
    #region ConfirmType
    public static object? ConfirmType(
        this object? actual,
        Type expected,
        string? message = null
    )
    {
        if (actual?.GetType() == expected)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected object of type {1}, but got {2}.",
            nameof(ConfirmType),
            null,
            expected.Name,
            actual?.GetType().Name,
            message
        );
    }

    public static T ConfirmType<T>(this object? actual, string? message = null)
    {
        _ = actual.ConfirmType(typeof(T), message);
        return (T)actual!;
    }
    #endregion ConfirmType

    #region ConfirmNotType
    public static object? ConfirmNotType(
        this object? actual,
        Type expected,
        string? message = null
    )
    {
        if (actual?.GetType() != expected)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected object not to be of type {1}.",
            nameof(ConfirmNotType),
            null,
            expected.Name,
            null,
            message
        );
    }

    public static object? ConfirmNotType<T>(
        this object? actual,
        string? message = null
    )
    {
        return actual.ConfirmNotType(typeof(T), message);
    }
    #endregion ConfirmNotType

    #region ConfirmInstanceOf
    public static TExpected? ConfirmInstanceOf<TExpected>(
        this object? actual,
        string? message = null
    )
    {
        return (TExpected?)actual.ConfirmInstanceOf(typeof(TExpected), message);
    }

    public static object? ConfirmInstanceOf(
        this object? actual,
        Type expected,
        string? message = null
    )
    {
        return expected.IsAssignableFrom(actual?.GetType()) == true
            ? actual
            : throw new ConfirmAssertException(
            "Expected {1} to be an instance of {2}.",
            nameof(ConfirmInstanceOf),
            null,
            new AutomaticFormatter().Format(actual),
            expected.Name,
            message
        );
    }

    public static object? ConfirmNotInstanceOf<TExpected>(
        this object? actual,
        string? message = null
    )
    {
        return actual.ConfirmNotInstanceOf(typeof(TExpected), message);
    }

    public static object? ConfirmNotInstanceOf(
        this object? actual,
        Type expected,
        string? message = null
    )
    {
        return !expected.IsAssignableFrom(actual?.GetType())
            ? actual
            : throw new ConfirmAssertException(
            "Expected {1} not to be an instance of {2}.",
            nameof(ConfirmNotInstanceOf),
            null,
            new AutomaticFormatter().Format(actual),
            expected.Name,
            message
        );
    }
    #endregion ConfirmInstanceOf
}
