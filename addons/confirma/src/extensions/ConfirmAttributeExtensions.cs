using System;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmAttributeExtensions
{
    #region ConfirmIsDecoratedWith
    public static Type ConfirmIsDecoratedWith(
        this Type actual,
        Type expected,
        string? message = null
    )
    {
        if (actual.IsDefined(expected, false))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected {1} to be decorated with {2}.",
            nameof(ConfirmIsDecoratedWith),
            null,
            expected.Name,
            actual.Name,
            message
        );
    }

    public static Type ConfirmIsDecoratedWith<T>(
        this Type actual,
        string? message = null
    )
    where T : Attribute
    {
        return ConfirmIsDecoratedWith(actual, typeof(T), message);
    }
    #endregion ConfirmIsDecoratedWith

    #region ConfirmIsNotDecoratedWith
    public static Type ConfirmIsNotDecoratedWith(
        this Type actual,
        Type expected,
        string? message = null
    )
    {
        if (!actual.IsDefined(expected, false))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected {1} not to be decorated with {2}.",
            nameof(ConfirmIsNotDecoratedWith),
            null,
            actual.Name,
            expected.Name,
            message
        );
    }

    public static Type ConfirmIsNotDecoratedWith<T>(
        this Type actual,
        string? message = null
    )
    where T : Attribute
    {
        return ConfirmIsNotDecoratedWith(actual, typeof(T), message);
    }
    #endregion ConfirmIsNotDecoratedWith
}
