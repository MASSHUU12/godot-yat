using System;
using Confirma.Exceptions;
using Confirma.Formatters;

namespace Confirma.Extensions;

public static class ConfirmArrayExtensions
{
    public static T[] ConfirmSize<T>(
        this T[] array,
        int expectedSize,
        string? message = null
    )
    {
        if (array.Length == expectedSize)
        {
            return array;
        }

        throw new ConfirmAssertException(
            "Expected array of size {1}, but {2} provided.",
            nameof(ConfirmSize),
            new NumericFormatter(),
            expectedSize,
            array.Length,
            message
        );
    }

    public static T[] ConfirmEmpty<T>(this T[] array, string? message = null)
    {
        return array.Length == 0
            ? array
            : throw new ConfirmAssertException(
                "Expected empty array, {2} elements provided.",
                nameof(ConfirmEmpty),
                new NumericFormatter(),
                null,
                array.Length,
                message
            );
    }

    public static T[] ConfirmNotEmpty<T>(this T[] array, string? message = null)
    {
        return array.Length > 0
            ? array
            : throw new ConfirmAssertException(
                "Expected non-empty array.",
                nameof(ConfirmNotEmpty),
                null,
                null,
                null,
                message
            );
    }

    public static T[] ConfirmContains<T>(
        this T[] array,
        T expected,
        string? message = null
    )
    {
        if (Array.IndexOf(array, expected) != -1)
        {
            return array;
        }

        throw new ConfirmAssertException(
            "Expected array to contain: {1}.",
            nameof(ConfirmContains),
            new AutomaticFormatter(),
            expected,
            null,
            message
        );
    }

    public static T[] ConfirmNotContains<T>(
        this T[] array,
        T expected,
        string? message = null
    )
    {
        if (Array.IndexOf(array, expected) == -1)
        {
            return array;
        }

        throw new ConfirmAssertException(
            "Expected array to not contain: {1}.",
            nameof(ConfirmNotContains),
            new AutomaticFormatter(),
            expected,
            null,
            message
        );
    }
}
