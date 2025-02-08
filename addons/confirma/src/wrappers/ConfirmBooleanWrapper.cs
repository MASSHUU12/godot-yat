using Confirma.Extensions;

namespace Confirma.Wrappers;

public partial class ConfirmBooleanWrapper : WrapperBase
{
    public static bool ConfirmTrue(bool actual, string? message = null)
    {
        CallAssertion(
            () => actual.ConfirmTrue(ParseMessage(message))
        );

        return actual;
    }

    public static bool ConfirmFalse(bool actual, string? message = null)
    {
        CallAssertion(
            () => actual.ConfirmFalse(ParseMessage(message))
        );

        return actual;
    }
}
