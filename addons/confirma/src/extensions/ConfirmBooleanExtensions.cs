using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmBooleanExtensions
{
    public static bool ConfirmTrue(this bool actual, string? message = null)
    {
        return actual
            ? actual
            : throw new ConfirmAssertException(message ?? "Expected true but was false.");
    }

    public static bool ConfirmFalse(this bool actual, string? message = null)
    {
        return !actual
            ? actual
            : throw new ConfirmAssertException(message ?? "Expected false but was true.");
    }
}
