using Confirma.Extensions;

namespace Confirma.Wrappers;

public partial class ConfirmNumericWrapper : WrapperBase
{
    public static long ConfirmIsPositive(
        long actual,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmIsPositive(ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmIsNotPositive(
        long actual,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmIsNotPositive(ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmIsNegative(
        long actual,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmIsNegative(ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmIsNotNegative(
        long actual,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmIsNotNegative(ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmSign(
        long actual,
        bool sign,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmSign(sign, ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmIsZero(
        long actual,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmIsZero(ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmIsNotZero(
        long actual,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmIsNotZero(ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmIsOdd(
        long actual,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmIsOdd(ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmIsEven(
        long actual,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmIsEven(ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmCloseTo(
        long actual,
        long expected,
        long tolerance,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmCloseTo(
                expected,
                tolerance,
                ParseMessage(message)
            )
        );

        return actual;
    }

    public static double ConfirmCloseToDouble(
        double actual,
        double expected,
        double tolerance,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmCloseTo(
                expected,
                tolerance,
                ParseMessage(message)
            )
        );

        return actual;
    }
}
