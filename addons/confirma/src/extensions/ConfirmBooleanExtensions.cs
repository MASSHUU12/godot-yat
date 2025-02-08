using Confirma.Exceptions;
using Confirma.Formatters;

namespace Confirma.Extensions;

public static class ConfirmBooleanExtensions
{
    public static bool ConfirmTrue(this bool actual, string? message = null)
    {
        if (actual)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            Formatter.DefaultFormat,
            nameof(ConfirmTrue),
            null,
            true,
            actual,
            message
        );
    }

    public static bool ConfirmFalse(this bool actual, string? message = null)
    {
        if (!actual)
        {
            return actual;
        }

        throw new ConfirmAssertException(
            Formatter.DefaultFormat,
            nameof(ConfirmFalse),
            null,
            false,
            actual,
            message
        );
    }
}
