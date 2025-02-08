using Confirma.Extensions;

namespace Confirma.Wrappers;

public partial class ConfirmStringWrapper : WrapperBase
{
    public static string ConfirmEmpty(
        string actual,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmEmpty(ParseMessage(message))
        );

        return actual;
    }

    public static string ConfirmNotEmpty(
        string actual,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmNotEmpty(ParseMessage(message))
        );

        return actual;
    }

    public static string ConfirmContains(
        string actual,
        string expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmContains(
                expected,
                message: ParseMessage(message)
            )
        );

        return actual;
    }

    public static string ConfirmNotContains(
        string actual,
        string expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmNotContains(expected, ParseMessage(message))
        );

        return actual;
    }

    public static string ConfirmStartsWith(
        string actual,
        string expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmStartsWith(expected, ParseMessage(message))
        );

        return actual;
    }

    public static string ConfirmNotStartsWith(
        string actual,
        string expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmNotStartsWith(expected, ParseMessage(message))
        );

        return actual;
    }

    public static string ConfirmEndsWith(
        string actual,
        string expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmEndsWith(expected, ParseMessage(message))
        );

        return actual;
    }

    public static string ConfirmNotEndsWith(
        string actual,
        string expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmNotEndsWith(expected, ParseMessage(message))
        );

        return actual;
    }

    public static string ConfirmHasLength(
        string actual,
        int expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmHasLength(expected, ParseMessage(message))
        );

        return actual;
    }

    public static string ConfirmNotHasLength(
        string actual,
        int expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmNotHasLength(expected, ParseMessage(message))
        );

        return actual;
    }

    public static string ConfirmEqualsCaseInsensitive(
        string actual,
        string expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmEqualsCaseInsensitive(
                expected,
                ParseMessage(message)
            )
        );

        return actual;
    }

    public static string ConfirmNotEqualsCaseInsensitive(
        string actual,
        string expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmNotEqualsCaseInsensitive(
                expected,
                ParseMessage(message)
            )
        );

        return actual;
    }

    public static string ConfirmMatchesPattern(
        string actual,
        string pattern,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmMatchesPattern(pattern, ParseMessage(message))
        );

        return actual;
    }

    public static string ConfirmDoesNotMatchPattern(
        string actual,
        string pattern,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmDoesNotMatchPattern(
                pattern,
                ParseMessage(message)
            )
        );

        return actual;
    }

    public static string ConfirmLowercase(
        string actual,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmLowercase(ParseMessage(message))
        );

        return actual;
    }

    public static string ConfirmUppercase(
        string actual,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmUppercase(ParseMessage(message))
        );

        return actual;
    }
}
