using System;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmUuidExtensions
{
    public static string? ConfirmValidUuid4(
        this string? actual,
        string? message = null
    )
    {
        if (Guid.TryParse(actual, out Guid _))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected a valid UUID, but got {2}.",
            nameof(ConfirmValidUuid4),
            null,
            null,
            actual,
            message
        );
    }

    public static string? ConfirmInvalidUuid4(
        this string? actual,
        string? message = null
    )
    {
        if (!Guid.TryParse(actual, out Guid _))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected invalid UUID, but got {2}.",
            nameof(ConfirmInvalidUuid4),
            null,
            null,
            actual,
            message
        );
    }
}
