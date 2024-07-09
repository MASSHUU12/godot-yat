using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmBooleanExtensions
{
    public static bool ConfirmTrue(this bool actual, string? message = null)
    {
        if (actual) return actual;

        throw new ConfirmAssertException(message ?? "Expected true but was false.");
    }

    public static bool ConfirmFalse(this bool actual, string? message = null)
    {
        if (!actual) return actual;

        throw new ConfirmAssertException(message ?? "Expected false but was true.");
    }
}
