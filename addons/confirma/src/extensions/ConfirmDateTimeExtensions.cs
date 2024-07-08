using System;

namespace Confirma.Extensions;

public static class ConfirmDateTimeExtensions
{
    public static DateTime ConfirmIsBefore(this DateTime actual, DateTime dateTime, string? message = null)
    {
        return actual.ConfirmLessThan(
            dateTime,
            message ?? $"{actual.ToUniversalTime()} is not before {dateTime.ToUniversalTime()}."
        );
    }

    public static DateTime ConfirmIsOnOrBefore(this DateTime actual, DateTime dateTime, string? message = null)
    {
        return actual.ConfirmLessThanOrEqual(
            dateTime,
            message ?? $"{actual.ToUniversalTime()} is not on or before {dateTime.ToUniversalTime()}."
        );
    }

    public static DateTime ConfirmIsAfter(this DateTime actual, DateTime dateTime, string? message = null)
    {
        return actual.ConfirmGreaterThan(
            dateTime,
            message ?? $"{actual.ToUniversalTime()} is not after {dateTime.ToUniversalTime()}."
        );
    }

    public static DateTime ConfirmIsOnOrAfter(this DateTime actual, DateTime dateTime, string? message = null)
    {
        return actual.ConfirmGreaterThanOrEqual(
            dateTime,
            message ?? $"{actual.ToUniversalTime()} is not on or after {dateTime.ToUniversalTime()}."
        );
    }
}
