using System;
using System.Threading.Tasks;
using Confirma.Exceptions;
using Confirma.Formatters;

namespace Confirma.Extensions;

public static class ConfirmActionExtensions
{
    public static Action ConfirmCompletesWithin(
        this Action action,
        TimeSpan timeSpan,
        string? message = null
    )
    {
        return !Task.Run(action).Wait(timeSpan)
            ? throw new ConfirmAssertException(
                "Expected action to complete within {1} ms, but it timed out.",
                nameof(ConfirmCompletesWithin),
                new NumericFormatter(2),
                timeSpan.TotalMilliseconds,
                null,
                message
            )
            : action;
    }

    public static Action ConfirmDoesNotCompleteWithin(
        this Action action,
        TimeSpan timeSpan,
        string? message = null
    )
    {
        try
        {
            _ = ConfirmCompletesWithin(action, timeSpan, message);
        }
        catch (ConfirmAssertException)
        {
            return action;
        }

        throw new ConfirmAssertException(
            "Expected action to not complete within {1} ms, but it did.",
            nameof(ConfirmDoesNotCompleteWithin),
            new NumericFormatter(2),
            timeSpan.TotalMilliseconds,
            null,
            message
        );
    }
}
