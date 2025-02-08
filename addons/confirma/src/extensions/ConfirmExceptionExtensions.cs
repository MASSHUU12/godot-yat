using System;
using Confirma.Exceptions;
using Confirma.Formatters;

namespace Confirma.Extensions;

public static class ConfirmExceptionExtensions
{
    #region ConfirmThrows
    public static Func<T> ConfirmThrows<T>(
        this Func<T> action,
        Type e,
        string? message = null
    )
    {
        try
        {
            _ = action();
        }
        catch (Exception ex)
        {
            if (ex.GetType() == e)
            {
                return action;
            }

            throw new ConfirmAssertException(
                "Expected {1} to be thrown instead of {2}.",
                nameof(ConfirmThrows),
                null,
                e.Name,
                ex.GetType().Name,
                message
            );
        }

        throw new ConfirmAssertException(
            "Expected {1} to be thrown, but no exception was thrown.",
            nameof(ConfirmThrows),
            null,
            e.Name,
            null,
            message
        );
    }

    public static Func<object?> ConfirmThrows<TE>(
        this Func<object?> action,
        string? message = null
    )
    where TE : Exception
    {
        return action.ConfirmThrows(typeof(TE), message);
    }

    public static Action ConfirmThrows<TE>(
        this Action action,
        string? message = null
    )
    {
        Func<object> func = () =>
        {
            action();
            return new object();
        };

        _ = func.ConfirmThrows(typeof(TE), message);

        return action;
    }
    #endregion ConfirmThrows

    #region ConfirmNotThrows
    public static Func<T> ConfirmNotThrows<T>(
        this Func<T> action,
        Type e,
        string? message = null
    )
    {
        try
        {
            _ = action();
        }
        catch (Exception ex)
        {
            if (ex.GetType() == e)
            {
                throw new ConfirmAssertException(
                    "Expected {1} not to be thrown.",
                    nameof(ConfirmNotThrows),
                    null,
                    e.Name,
                    null,
                    message
                );
            }
        }

        return action;
    }

    public static Func<object?> ConfirmNotThrows<TE>(
        this Func<object?> action,
        string? message = null
    )
    where TE : Exception
    {
        return ConfirmNotThrows(action, typeof(TE), message);
    }

    public static Action ConfirmNotThrows<TE>(
        this Action action,
        string? message = null
    )
    {
        Func<object> func = () =>
        {
            action();
            return new object();
        };

        _ = func.ConfirmNotThrows(typeof(TE), message);

        return action;
    }
    #endregion ConfirmNotThrows

    #region ConfirmThrowsWMessage
    public static Func<T> ConfirmThrowsWMessage<T>(
        this Func<T> action,
        Type e,
        string exMessage,
        string? message = null
    )
    {
        try
        {
            _ = action();
        }
        catch (Exception ex)
        {
            if (ex.GetType() == e && ex.Message == exMessage)
            {
                return action;
            }

            if (ex.GetType() != e && ex.Message != exMessage)
            {
                throw new ConfirmAssertException(
                    $"Expected {e.Name} to be thrown with message "
                    + "{1}, but got {2} "
                    + $"{ex.GetType().Name} instead.",
                    nameof(ConfirmThrowsWMessage),
                    new StringFormatter(),
                    exMessage,
                    string.IsNullOrEmpty(ex.Message)
                        ? "without a message"
                        : $"with message \"{ex.Message}\"",
                    message
                );
            }

            if (ex.GetType() != e)
            {
                throw new ConfirmAssertException(
                    "Expected {1} to be thrown, but got {2} instead.",
                    nameof(ConfirmThrowsWMessage),
                    null,
                    e.Name,
                    ex.GetType().Name,
                    message
                );
            }

            if (ex.Message != exMessage)
            {
                throw new ConfirmAssertException(
                    $"Expected {e.Name} to be thrown with message "
                    + "{1} but got {2} instead.",
                    nameof(ConfirmThrowsWMessage),
                    new StringFormatter(),
                    exMessage,
                    ex.Message,
                    message
                );
            }
        }

        throw new ConfirmAssertException(
            "Expected {1} to be thrown, but no exception was thrown.",
            nameof(ConfirmThrowsWMessage),
            null,
            e.Name,
            null,
            message
        );
    }

    public static Func<object?> ConfirmThrowsWMessage<TE>(
        this Func<object?> action,
        string exMessage,
        string? message = null
    )
    where TE : Exception
    {
        return action.ConfirmThrowsWMessage(typeof(TE), exMessage, message);
    }

    public static Action ConfirmThrowsWMessage<TE>(
        this Action action,
        string exMessage,
        string? message = null
    )
    where TE : Exception
    {
        Func<object> func = () =>
        {
            action();
            return new object();
        };

        _ = func.ConfirmThrowsWMessage(typeof(TE), exMessage, message);

        return action;
    }
    #endregion ConfirmThrowsWMessage

    #region ConfirmNotThrowsWMessage
    public static Func<T> ConfirmNotThrowsWMessage<T>(
        this Func<T> action,
        Type e,
        string exMessage,
        string? message = null
    )
    {
        try
        {
            _ = action();
        }
        catch (Exception ex)
        {
            if (ex.GetType() == e)
            {
                throw new ConfirmAssertException(
                    $"Expected {e.Name} not to be thrown with message "
                    + "{1}.",
                    nameof(ConfirmNotThrowsWMessage),
                    new StringFormatter(),
                    exMessage,
                    null,
                    message
                );
            }
        }

        return action;
    }

    public static Func<object?> ConfirmNotThrowsWMessage<TE>(
        this Func<object?> action,
        string exMessage,
        string? message = null
    )
    where TE : Exception
    {
        return ConfirmNotThrowsWMessage(action, typeof(TE), exMessage, message);
    }

    public static Action ConfirmNotThrowsWMessage<TE>(
        this Action action,
        string exMessage,
        string? message = null
    )
    where TE : Exception
    {
        Func<object> func = () =>
        {
            action();
            return new object();
        };

        _ = func.ConfirmNotThrowsWMessage(typeof(TE), exMessage, message);

        return action;
    }
    #endregion ConfirmNotThrowsWMessage
}
