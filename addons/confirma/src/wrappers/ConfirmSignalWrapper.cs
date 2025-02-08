using Confirma.Extensions;
using Godot;

namespace Confirma.Wrappers;

public partial class ConfirmSignalWrapper : WrapperBase
{
    public static GodotObject ConfirmSignalExists(
        GodotObject actual,
        StringName signalName,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmSignalExists(
                signalName,
                ParseMessage(message)
            )
        );

        return actual;
    }

    public static GodotObject ConfirmSignalDoesNotExist(
        GodotObject actual,
        StringName signalName,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmSignalDoesNotExist(
                signalName,
                ParseMessage(message)
            )
        );

        return actual;
    }
}
