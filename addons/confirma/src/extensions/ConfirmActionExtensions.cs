using System;
using System.Threading.Tasks;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmActionExtensions
{
    public static Action ConfirmCompletesWithin(this Action action, TimeSpan timeSpan, string? message = null)
    {
        Task task = Task.Run(action);

        return !task.Wait(timeSpan)
            ? throw new ConfirmAssertException(
                message ??
                $"Action did not complete within {timeSpan.TotalMilliseconds} ms."
            )
            : action;
    }

    public static Action ConfirmDoesNotCompleteWithin(this Action action, TimeSpan timeSpan, string? message = null)
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
            message ??
            $"Action completed within {timeSpan.TotalMilliseconds} ms, but it should not have."
        );
    }
}
