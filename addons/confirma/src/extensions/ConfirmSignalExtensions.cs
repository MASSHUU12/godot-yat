using Confirma.Exceptions;
using Godot;

namespace Confirma.Extensions;

public static class ConfirmSignalExtensions
{
    #region ConfirmSignalExists
    public static GodotObject ConfirmSignalExists(
        this GodotObject actual,
        StringName signalName,
        string? message = null
    )
    {
        if (actual.HasSignal(signalName))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected signal {1} to exist on object of type {2}.",
            nameof(ConfirmSignalExists),
            null,
            signalName,
            actual.GetType().Name,
            message
        );
    }

    public static GodotObject ConfirmSignalDoesNotExist(
        this GodotObject actual,
        StringName signalName,
        string? message = null
    )
    {
        if (!actual.HasSignal(signalName))
        {
            return actual;
        }

        throw new ConfirmAssertException(
            "Expected signal {1} to not exist on object of type {2}.",
            nameof(ConfirmSignalDoesNotExist),
            null,
            signalName,
            actual.GetType().Name,
            message
        );
    }
    #endregion ConfirmSignalExists
}
