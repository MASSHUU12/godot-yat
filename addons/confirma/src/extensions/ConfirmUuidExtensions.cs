using System;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmUuidExtensions
{
    public static string? ConfirmValidUuid4(this string? actual, string? message = null)
    {
        if (Guid.TryParse(actual, out var _)) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected a valid UUID, but '{actual}' is not in the correct format."
        );
    }

    public static string? ConfirmInvalidUuid4(this string? actual, string? message = null)
    {
        if (!Guid.TryParse(actual, out var _)) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected '{actual}' to not be a valid UUID."
        );
    }
}
