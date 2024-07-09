using System.Linq;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmEqualExtensions
{
    public static T? ConfirmEqual<T>(this T? actual, T? expected, string? message = null)
    {
        if (!(actual?.Equals(expected)) ?? false)
        {
            throw new ConfirmAssertException(
                message ??
                $"Expected '{expected}' but got '{actual}'."
            );
        }

        return actual;
    }

    public static T?[] ConfirmEqual<T>(this T?[] actual, T?[] expected, string? message = null)
    {
        if (actual.SequenceEqual(expected)) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected '{string.Join(", ", expected)}' but got '{string.Join(", ", actual)}'."
        );
    }

    public static T? ConfirmNotEqual<T>(this T? actual, T? expected, string? message = null)
    {
        if (actual?.Equals(expected) ?? false)
        {
            throw new ConfirmAssertException(
                message ??
                $"Expected not '{expected}' but got '{actual}'."
            );
        }

        return actual;
    }

    public static T?[] ConfirmNotEqual<T>(this T?[] actual, T?[] expected, string? message = null)
    {
        if (!actual.SequenceEqual(expected)) return actual;

        throw new ConfirmAssertException(
            message ??
            $"Expected not '{string.Join(", ", expected)}' but got '{string.Join(", ", actual)}'."
        );
    }
}
