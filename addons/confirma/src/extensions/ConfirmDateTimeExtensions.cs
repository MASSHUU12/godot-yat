using System;
using Confirma.Formatters;
using Confirma.Helpers;

namespace Confirma.Extensions;

public static class ConfirmDateTimeExtensions
{
    public static DateTime ConfirmIsBefore(
        this DateTime actual,
        DateTime dateTime,
        string? message = null
    )
    {
        return actual.ConfirmLessThan(
            dateTime,
            message ?? new AssertionMessageGenerator(
                "Expected {1} to be before {2}.",
                nameof(ConfirmIsBefore),
                new DefaultFormatter(),
                dateTime.ToUniversalTime(),
                actual.ToUniversalTime()
            ).GenerateMessage()
        );
    }

    public static DateTime ConfirmIsOnOrBefore(
        this DateTime actual,
        DateTime dateTime,
        string? message = null
    )
    {
        return actual.ConfirmLessThanOrEqual(
            dateTime,
            message ?? new AssertionMessageGenerator(
                "Expected {1} to be on or before {2}.",
                nameof(ConfirmIsOnOrBefore),
                new DefaultFormatter(),
                dateTime.ToUniversalTime(),
                actual.ToUniversalTime()
            ).GenerateMessage()
        );
    }

    public static DateTime ConfirmIsAfter(
        this DateTime actual,
        DateTime dateTime,
        string? message = null
    )
    {
        return actual.ConfirmGreaterThan(
            dateTime,
            message ?? new AssertionMessageGenerator(
                "Expected {1} to be after {2}.",
                nameof(ConfirmIsAfter),
                new DefaultFormatter(),
                dateTime.ToUniversalTime(),
                actual.ToUniversalTime()
            ).GenerateMessage()
        );
    }

    public static DateTime ConfirmIsOnOrAfter(
        this DateTime actual,
        DateTime dateTime,
        string? message = null
    )
    {
        return actual.ConfirmGreaterThanOrEqual(
            dateTime,
            message ?? new AssertionMessageGenerator(
                "Expected {1} to be on or after {2}.",
                nameof(ConfirmIsOnOrAfter),
                new DefaultFormatter(),
                dateTime.ToUniversalTime(),
                actual.ToUniversalTime()
            ).GenerateMessage()
        );
    }
}
