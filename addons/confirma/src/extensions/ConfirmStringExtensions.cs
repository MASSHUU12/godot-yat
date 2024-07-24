using System;
using System.Text.RegularExpressions;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmStringExtensions
{
    #region ConfirmEmpty
    public static string? ConfirmEmpty(
        this string? actual,
        string? message = null
    )
    {
        if (string.IsNullOrEmpty(actual))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected empty string but found: {actual}"
        );
    }

    public static string? ConfirmNotEmpty(
        this string? actual,
        string? message = null
    )
    {
        if (!string.IsNullOrEmpty(actual))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected non-empty string but found: {actual}"
        );
    }
    #endregion ConfirmEmpty

    #region ConfirmContains
    public static string? ConfirmContains(
        this string? actual,
        string expected,
        StringComparison comparisonType = StringComparison.OrdinalIgnoreCase,
        string? message = null
    )
    {
        if (actual?.Contains(expected, comparisonType) == true)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected string to contain: {expected} but found: {actual}"
        );
    }

    public static string? ConfirmNotContains(this string? actual, string expected, string? message = null)
    {
        if (actual?.Contains(expected) == false)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected string to not contain: {expected} but found: {actual}"
        );
    }
    #endregion ConfirmContains

    #region ConfirmStartsWith
    public static string? ConfirmStartsWith(this string? actual, string expected, string? message = null)
    {
        if (actual?.StartsWith(expected, StringComparison.InvariantCulture) == true)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected string to start with: {expected} but found: {actual}"
        );
    }

    public static string? ConfirmNotStartsWith(
        this string? actual,
        string expected,
        string? message = null
    )
    {
        if (actual?.StartsWith(expected, StringComparison.InvariantCulture) == false)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected string to not start with: {expected} but found: {actual}"
        );
    }
    #endregion ConfirmStartsWith

    #region ConfirmEndsWith
    public static string? ConfirmEndsWith(
        this string? actual,
        string expected,
        string? message = null
    )
    {
        if (actual?.EndsWith(expected, StringComparison.InvariantCulture) == true)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected string to end with: {expected} but found: {actual}"
        );
    }

    public static string? ConfirmNotEndsWith(
        this string? actual,
        string expected,
        string? message = null
    )
    {
        if (actual?.EndsWith(expected, StringComparison.InvariantCulture) == false)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected string to not end with: {expected} but found: {actual}"
        );
    }
    #endregion ConfirmEndsWith

    #region ConfirmHasLength
    public static string? ConfirmHasLength(
        this string? actual,
        int expected,
        string? message = null
    )
    {
        if (actual?.Length == expected)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected string to have length: {expected} but found: {actual?.Length}"
        );
    }

    public static string? ConfirmNotHasLength(
        this string? actual,
        int expected,
        string? message = null
    )
    {
        if (actual?.Length != expected)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected string to not have length: {expected} but found: {actual?.Length}"
        );
    }
    #endregion ConfirmHasLength

    #region ConfirmEqualsCaseInsensitive
    public static string? ConfirmEqualsCaseInsensitive(
        this string? actual,
        string expected,
        string? message = null
    )
    {
        if (string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected string to equal: {expected} but found: {actual}"
        );
    }

    public static string? ConfirmNotEqualsCaseInsensitive(
        this string? actual,
        string expected,
        string? message = null
    )
    {
        if (!string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected string to not equal: {expected} but found: {actual}"
        );
    }
    #endregion ConfirmEqualsCaseInsensitive

    #region ConfirmMatchesPattern
    public static string ConfirmMatchesPattern(
        this string value,
        string pattern,
        string? message = null
    )
    {
        if (Regex.IsMatch(value, pattern))
        {
            return value;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected string to match pattern '{pattern}'."
        );
    }

    public static string ConfirmDoesNotMatchPattern(
        this string value,
        string pattern,
        string? message = null
    )
    {
        if (!Regex.IsMatch(value, pattern))
        {
            return value;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected string to not match pattern '{pattern}'."
        );
    }
    #endregion ConfirmMatchesPattern

    public static bool ConfirmLowercase(this string value, string? message = null)
    {
        if (value.Equals(value.ToLowerInvariant(), StringComparison.Ordinal))
        {
            return true;
        }

        throw new ConfirmAssertException(
            message
            ?? $"Expected {value} to be lowercase."
        );
    }

    public static bool ConfirmUppercase(this string value, string? message = null)
    {
        if (value.Equals(value.ToUpperInvariant(), StringComparison.Ordinal))
        {
            return true;
        }

        throw new ConfirmAssertException(
            message
            ?? $"Expected {value} to be uppercase."
        );
    }
}
