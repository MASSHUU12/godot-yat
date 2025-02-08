using Confirma.Exceptions;
using Confirma.Formatters;

namespace Confirma.Extensions;

public static class ConfirmReferenceExtensions
{
    public static object ConfirmSameReference(
        this object obj1,
        object obj2,
        string? message = null
    )
    {
        if (ReferenceEquals(obj1, obj2))
        {
            return obj1;
        }

        throw new ConfirmAssertException(
            "Expected {1} and {2} to be of the same reference.",
            nameof(ConfirmSameReference),
            new AutomaticFormatter(),
            obj1,
            obj2,
            message
        );
    }

    public static object ConfirmDifferentReference(
        this object obj1,
        object obj2,
        string? message = null
    )
    {
        if (!ReferenceEquals(obj1, obj2))
        {
            return obj1;
        }

        throw new ConfirmAssertException(
            "Expected {1} and {2} to be of different references.",
            nameof(ConfirmDifferentReference),
            new AutomaticFormatter(),
            obj1,
            obj2,
            message
        );
    }
}
