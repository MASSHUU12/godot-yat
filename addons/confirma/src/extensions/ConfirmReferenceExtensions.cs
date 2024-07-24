using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmReferenceExtensions
{
    public static object ConfirmSameReference(this object obj1, object obj2, string? message = "")
    {
        if (ReferenceEquals(obj1, obj2))
        {
            return obj1;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected {obj1} and {obj2} to have the same reference."
        );
    }

    public static object ConfirmDifferentReference(this object obj1, object obj2, string? message = "")
    {
        if (!ReferenceEquals(obj1, obj2))
        {
            return obj1;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected {obj1} and {obj2} to have different references."
        );
    }

}
