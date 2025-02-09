using Confirma.Attributes;
using Confirma.Extensions;
using YAT.Helpers;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class NumericTest
{
    #region IsWithinRange
    [TestCase(5, 0, 10)]
    [TestCase(0, 0, 10)]
    [TestCase(10, 0, 10)]
    public static void IsWithinRange_WhenIsWithinRange(
        int value,
        int min,
        int max
    )
    {
        _ = value.IsWithinRange<int>(min, max).ConfirmTrue();
    }

    [TestCase(11, -5, 10)]
    [TestCase(-1, 0, 9)]
    [TestCase(5, 6, 11)]
    public static void IsWithinRange_WhenIsNotWithinRange(
        int value,
        int min,
        int max
    )
    {
        _ = value.IsWithinRange<int>(min, max).ConfirmFalse();
    }

    [TestCase(5d, 0d, 10d)]
    [TestCase(0d, 0d, 10d)]
    [TestCase(10d, 0d, 10d)]
    public static void IsWithinRange_WhenIsWithinRange(
        double value,
        double min,
        double max
    )
    {
        _ = value.IsWithinRange<double>(min, max).ConfirmTrue();
    }

    [TestCase(11d, -5d, 10d)]
    [TestCase(-1d, 0d, 9d)]
    [TestCase(5d, 6d, 11d)]
    public static void IsWithinRange_WhenIsNotWithinRange(
        double value,
        double min,
        double max
    )
    {
        _ = value.IsWithinRange<double>(min, max).ConfirmFalse();
    }
    #endregion IsWithinRange

    #region IsWithinRange_TE
    [TestCase(5, 0f, 10f)]
    [TestCase(0, 0f, 10f)]
    [TestCase(10, 0f, 10f)]
    public static void IsWithinRange_TE_WhenIsWithinRange(
        int value,
        float min,
        float max
    )
    {
        _ = Numeric.IsWithinRange(value, min, max).ConfirmTrue();
    }

    [TestCase(11, -5f, 10f)]
    [TestCase(-1, 0f, 9f)]
    [TestCase(5, 6f, 11f)]
    public static void IsWithinRange_TE_WhenIsNotWithinRange(
        int value,
        float min,
        float max
    )
    {
        _ = Numeric.IsWithinRange(value, min, max).ConfirmFalse();
    }
    #endregion IsWithinRange_TE

    #region TryConvert
    [TestCase("5", 5)]
    [TestCase("-5", -5)]
    [TestCase("0", 0)]
    public static void TryConvert_Int_Valid(string value, int expected)
    {
        _ = value.TryConvert(out int result).ConfirmTrue();
        _ = result.ConfirmEqual(expected);
    }

    [TestCase("5.0", 5.0)]
    [TestCase("-5.0", -5.0)]
    [TestCase("0.0", 0.0)]
    public static void TryConvert_Double_Valid(string value, double expected)
    {
        _ = value.TryConvert(out double result).ConfirmTrue();
        _ = result.ConfirmEqual(expected);
    }

    [TestCase("5five")]
    [TestCase("five5")]
    [TestCase("5.0f")]
    [TestCase("-5.0f")]
    [TestCase("5.0d")]
    [TestCase("-5.0d")]
    public static void TryConvert_Int_Invalid(string value)
    {
        _ = value.TryConvert(out int _).ConfirmFalse();
    }

    [TestCase("5.0five")]
    [TestCase("5.0dfive")]
    [TestCase("-5.0dfive")]
    [TestCase("5.0f")]
    [TestCase("-5.0f")]
    [TestCase("5.0d")]
    public static void TryConvert_Double_Invalid(string value)
    {
        _ = value.TryConvert(out double _).ConfirmFalse();
    }
    #endregion TryConvert

    [TestCase(0, 2, "0 B")]
    [TestCase(1, 2, "1 B")]
    [TestCase(1023, 2, "1023 B")]
    [TestCase(1024, 2, "1.00 KiB")]
    [TestCase(1025, 2, "1.00 KiB")]
    [TestCase(1024 * 1024, 2, "1.00 MiB")]
    [TestCase(1024 * 1024 * 1024, 2, "1.00 GiB")]
    [TestCase(2137420969, 2, "1.99 GiB")]
    [TestCase(2137420969, 6, "1.990628 GiB")]
    public static void SizeToString(long size, int precision, string expected)
    {
        _ = Numeric.SizeToString(size, precision).ConfirmEqual(expected);
    }
}
