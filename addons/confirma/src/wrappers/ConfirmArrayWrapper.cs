using Confirma.Extensions;
using Godot;
using Godot.Collections;

namespace Confirma.Wrappers;

public partial class ConfirmArrayWrapper : WrapperBase
{
    public static Array ConfirmSize(
        Array actual,
        int expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmCount(expected, ParseMessage(message))
        );

        return actual;
    }

    public static Array ConfirmEmpty(Array actual, string? message = null)
    {
        CallAssertion(
            () => actual.ConfirmEmpty(ParseMessage(message))
        );

        return actual;
    }

    public static Array ConfirmNotEmpty(Array actual, string? message = null)
    {
        CallAssertion(
            () => actual.ConfirmNotEmpty(ParseMessage(message))
        );

        return actual;
    }

    public static Array ConfirmContains(
        Array actual,
        Variant expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmContains(expected, ParseMessage(message))
        );

        return actual;
    }

    public static Array ConfirmNotContains(
        Array actual,
        Variant expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmNotContains(expected, ParseMessage(message))
        );

        return actual;
    }
}
