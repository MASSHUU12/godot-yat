using Confirma.Extensions;

namespace Confirma.Wrappers;

public partial class ConfirmRangeWrapper : WrapperBase
{
    public static long ConfirmInRange(
        long actual,
        long min,
        long max,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmInRange(min, max, ParseMessage(message))
        );

        return actual;
    }

    public static double ConfirmInRangeDouble(
        double actual,
        double min,
        double max,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmInRange(min, max, ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmNotInRange(
        long actual,
        long min,
        long max,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmNotInRange(min, max, ParseMessage(message))
        );

        return actual;
    }

    public static double ConfirmNotInRangeDouble(
        double actual,
        double min,
        double max,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmNotInRange(min, max, ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmGreaterThan(
        long actual,
        long value,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmGreaterThan(value, ParseMessage(message))
        );

        return actual;
    }

    public static double ConfirmGreaterThanDouble(
        double actual,
        double value,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmGreaterThan(value, ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmGreaterThanOrEqual(
        long actual,
        long value,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmGreaterThanOrEqual(value, ParseMessage(message))
        );

        return actual;
    }

    public static double ConfirmGreaterThanOrEqualDouble(
        double actual,
        double value,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmGreaterThanOrEqual(value, ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmLessThan(
        long actual,
        long value,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmLessThan(value, ParseMessage(message))
        );

        return actual;
    }

    public static double ConfirmLessThanDouble(
        double actual,
        double value,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmLessThan(value, ParseMessage(message))
        );

        return actual;
    }

    public static long ConfirmLessThanOrEqual(
        long actual,
        long value,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmLessThanOrEqual(value, ParseMessage(message))
        );

        return actual;
    }

    public static double ConfirmLessThanOrEqualDouble(
        double actual,
        double value,
        string? message = null
    )
    {
        CallAssertion(
            () => actual.ConfirmLessThanOrEqual(value, ParseMessage(message))
        );

        return actual;
    }
}
