using Confirma.Extensions;
using Godot;

namespace Confirma.Wrappers;

public partial class ConfirmEqualWrapper : WrapperBase
{
    public static GodotObject ConfirmEqual(
        GodotObject actual,
        GodotObject expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmEqual(expected, ParseMessage(message))
        );

        return actual;
    }

    public static GodotObject ConfirmNotEqual(
        GodotObject actual,
        GodotObject expected,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmNotEqual(expected, ParseMessage(message))
        );

        return actual;
    }
}
