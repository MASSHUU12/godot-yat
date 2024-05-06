using Confirma.Attributes;
using Confirma.Classes;
using Confirma.Extensions;
using YAT.Helpers;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public class NumericTest
{
	[TestCase(5, 0, 10, true)]
	[TestCase(0, 0, 10, true)]
	[TestCase(10, 0, 10, true)]
	[TestCase(11, 0, 10, false)]
	[TestCase(-1, 0, 10, false)]
	public static void IsWithingRange_Int(int value, int min, int max, bool expected)
	{
		Numeric.IsWithinRange<int>(value, min, max).ConfirmEqual(expected);
	}

	[TestCase(5d, 0d, 10d, true)]
	[TestCase(0d, 0d, 10d, true)]
	[TestCase(10d, 0d, 10d, true)]
	[TestCase(11d, 0d, 10d, false)]
	[TestCase(-1d, 0d, 10d, false)]
	public static void IsWithingRange_Double(double value, double min, double max, bool expected)
	{
		Numeric.IsWithinRange<double>(value, min, max).ConfirmEqual(expected);
	}

	[TestCase("5", 5, true)]
	[TestCase("-5", -5, true)]
	[TestCase("5.0", -1, false)]
	[TestCase("5.0f", -1, false)]
	[TestCase("-5.0", -1, false)]
	[TestCase("-5.0f", -1, false)]
	[TestCase("5five", -1, false)]
	[TestCase("five5", -1, false)]
	public static void TryConvert_Int(string value, int expected, bool success)
	{
		Numeric.TryConvert(value, out int result).ConfirmEqual(success);

		if (success) result.ConfirmEqual(expected);
	}

	[TestCase("5.0", 5.0, true)]
	[TestCase("-5.0", -5.0, true)]
	[TestCase("5five", -1, false)]
	[TestCase("5.0five", -1, false)]
	[TestCase("-5.0dfive", -1, false)]
	[TestCase("5.0f", -1, false)]
	[TestCase("-5.0f", -1, false)]
	[TestCase("5.0d", -1, false)]
	public static void TryConvert_Double(string value, double expected, bool success)
	{
		Numeric.TryConvert(value, out double result).ConfirmEqual(success);

		if (success) result.ConfirmEqual(expected);
	}

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
