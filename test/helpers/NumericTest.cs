using Confirma.Attributes;
using Confirma.Classes;
using Confirma.Extensions;
using YAT.Helpers;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public class NumericTest
{
	#region IsWithinRange
	[TestCase(5, 0, 10)]
	[TestCase(0, 0, 10)]
	[TestCase(10, 0, 10)]
	public static void IsWithinRange_WhenValueIsWithinRange(int value, int min, int max)
	{
		Numeric.IsWithinRange<int>(value, min, max).ConfirmTrue();
	}

	[TestCase(11, -5, 10)]
	[TestCase(-1, 0, 9)]
	[TestCase(5, 6, 11)]
	public static void IsWithinRange_WhenValueIsNotWithinRange(int value, int min, int max)
	{
		Numeric.IsWithinRange<int>(value, min, max).ConfirmFalse();
	}

	[TestCase(5d, 0d, 10d)]
	[TestCase(0d, 0d, 10d)]
	[TestCase(10d, 0d, 10d)]
	public static void IsWithinRange_WhenValueIsWithinRange(double value, double min, double max)
	{
		Numeric.IsWithinRange<double>(value, min, max).ConfirmTrue();
	}

	[TestCase(11d, -5d, 10d)]
	[TestCase(-1d, 0d, 9d)]
	[TestCase(5d, 6d, 11d)]
	public static void IsWithinRange_WhenValueIsNotWithinRange(double value, double min, double max)
	{
		Numeric.IsWithinRange<double>(value, min, max).ConfirmFalse();
	}
	#endregion

	#region IsWithinRange_TE
	[TestCase(5, 0f, 10f)]
	[TestCase(0, 0f, 10f)]
	[TestCase(10, 0f, 10f)]
	public static void IsWithinRange_TE_WhenValueIsWithinRange(int value, float min, float max)
	{
		Numeric.IsWithinRange(value, min, max).ConfirmTrue();
	}

	[TestCase(11, -5f, 10f)]
	[TestCase(-1, 0f, 9f)]
	[TestCase(5, 6f, 11f)]
	public static void IsWithinRange_TE_WhenValueIsNotWithinRange(int value, float min, float max)
	{
		Numeric.IsWithinRange(value, min, max).ConfirmFalse();
	}
	#endregion

	#region TryConvert
	[TestCase("5", 5)]
	[TestCase("-5", -5)]
	[TestCase("0", 0)]
	public static void TryConvert_WhenValueIsValid_Int(string value, int expected)
	{
		Numeric.TryConvert(value, out int result).ConfirmTrue();
		result.ConfirmEqual(expected);
	}

	[TestCase("5.0", 5.0)]
	[TestCase("-5.0", -5.0)]
	[TestCase("0.0", 0.0)]
	public static void TryConvert_WhenValueIsValid_Double(string value, double expected)
	{
		Numeric.TryConvert(value, out double result).ConfirmTrue();
		result.ConfirmEqual(expected);
	}

	[TestCase("5five")]
	[TestCase("five5")]
	[TestCase("5.0f")]
	[TestCase("-5.0f")]
	[TestCase("5.0d")]
	[TestCase("-5.0d")]
	public static void TryConvert_WhenValueIsInvalid_Int(string value)
	{
		Numeric.TryConvert(value, out int _).ConfirmFalse();
	}

	[TestCase("5.0five")]
	[TestCase("5.0dfive")]
	[TestCase("-5.0dfive")]
	[TestCase("5.0f")]
	[TestCase("-5.0f")]
	[TestCase("5.0d")]
	public static void TryConvert_WhenValueIsInvalid_Double(string value)
	{
		Numeric.TryConvert(value, out double _).ConfirmFalse();
	}
	#endregion

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
		Numeric.SizeToString(size, precision).ConfirmEqual(expected);
	}
}
