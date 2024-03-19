using YAT.Helpers;

namespace GdUnit4
{
	using static Assertions;

	[TestSuite]
	public partial class TestNumericHelper
	{
		#region TestIsWithinRange
		[TestCase(5, 0, 10, true)]
		[TestCase(0, 0, 10, true)]
		[TestCase(10, 0, 10, true)]
		[TestCase(11, 0, 10, false)]
		[TestCase(-1, 0, 10, false)]
		public void TestIsWithinRangeInt(int value, int min, int max, bool expected)
		{
			AssertThat(Numeric.IsWithinRange<int>(value, min, max)).IsEqual(expected);
		}

		[TestCase(0.3, 0.1, 0.5, true)]
		[TestCase(0.1, 0.1, 0.5, true)]
		[TestCase(0.5, 0.1, 0.5, true)]
		[TestCase(0.6, 0.1, 0.5, false)]
		[TestCase(0, 0.1, 0.5, false)]
		public void TestIsWithinRangeDouble(double value, double min, double max, bool expected)
		{
			AssertThat(Numeric.IsWithinRange<double>(value, min, max)).IsEqual(expected);
		}
		#endregion

		#region TestTryConvert
		[TestCase("5", 5, true)]
		[TestCase("-5", -5, true)]
		[TestCase("5.0", -1, false)]
		[TestCase("5.0f", -1, false)]
		[TestCase("-5.0", -1, false)]
		[TestCase("-5.0f", -1, false)]
		[TestCase("five", -1, false)]
		[TestCase("5five", -1, false)]
		[TestCase("five5", -1, false)]
		[TestCase("5five5", -1, false)]
		public void TestTryConvertInt(string value, int expectedResult, bool expected)
		{
			AssertThat(Numeric.TryConvert(value, out int i)).IsEqual(expected);

			if (expected) AssertThat(i).IsEqual(expectedResult);
		}

		[TestCase("5.0", 5.0, true)]
		[TestCase("-5.0", -5.0, true)]
		[TestCase("five", -1, false)]
		[TestCase("5five", -1, false)]
		[TestCase("five5", -1, false)]
		[TestCase("5five5", -1, false)]
		[TestCase("5.0five", -1, false)]
		[TestCase("-5.0dfive", -1, false)]
		[TestCase("5.0f", -1, false)]
		[TestCase("-5.0f", -1, false)]
		[TestCase("5.0d", -1, false)]
		[TestCase("-5.0d", -1, false)]
		public void TestTryConvertDouble(string value, double expectedResult, bool expected)
		{
			AssertThat(Numeric.TryConvert(value, out double d)).IsEqual(expected);

			if (expected) AssertThat(d).IsEqual(expectedResult);
		}
		#endregion

		#region TestSizeToString
		[TestCase(0, 2, "0 B")]
		[TestCase(1, 2, "1 B")]
		[TestCase(1023, 2, "1023 B")]
		[TestCase(1024, 2, "1.00 KiB")]
		[TestCase(1025, 2, "1.00 KiB")]
		[TestCase(1024 * 1024, 2, "1.00 MiB")]
		[TestCase(1024 * 1024 * 1024, 2, "1.00 GiB")]
		[TestCase(2137420969, 2, "1.99 GiB")]
		[TestCase(2137420969, 6, "1.990628 GiB")]
		public void TestSizeToString(long fileSize, int precision, string expected)
		{
			AssertThat(Numeric.SizeToString(fileSize, precision)).IsEqual(expected);
		}
		#endregion
	}
}
