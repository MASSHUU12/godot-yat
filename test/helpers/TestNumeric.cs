using Chickensoft.GoDotTest;
using Godot;
using YAT.Helpers;
using Shouldly;
using System;

namespace Test;

public class TestNumeric : TestClass
{
	public TestNumeric(Node testScene) : base(testScene) { }

	#region TestIsWithinRange
	[Test]
	public static void TestIsWithinRange()
	{
		IsWithinRange(5, 0, 10, true);
		IsWithinRange(0, 0, 10, true);
		IsWithinRange(10, 0, 10, true);
		IsWithinRange(11, 0, 10, false);
		IsWithinRange(-1, 0, 10, false);

		IsWithinRange(0.3, 0.1, 0.5, true);
		IsWithinRange(0.1, 0.1, 0.5, true);
		IsWithinRange(0.5, 0.1, 0.5, true);
		IsWithinRange(0.6, 0.1, 0.5, false);
		IsWithinRange(0, 0.1, 0.5, false);
	}

	private static void IsWithinRange<T>(T value, T min, T max, bool expected) where T : IComparable<T>
	{
		Numeric.IsWithinRange<T>(value, min, max).ShouldBe(expected);
	}
	#endregion

	#region TestTryConvert
	[Test]
	public static void TestTryConvert()
	{
		TryConvert("5", 5, true);
		TryConvert("-5", -5, true);
		TryConvert("5.0", -1, false);
		TryConvert("5.0f", -1, false);
		TryConvert("-5.0", -1, false);
		TryConvert("-5.0f", -1, false);
		TryConvert("five", -1, false);
		TryConvert("5five", -1, false);
		TryConvert("five5", -1, false);
		TryConvert("5five5", -1, false);

		TryConvert("5.0", 5.0, true);
		TryConvert("-5.0", -5.0, true);
		TryConvert("five", -1, false);
		TryConvert("5five", -1, false);
		TryConvert("five5", -1, false);
		TryConvert("5five5", -1, false);
		TryConvert("5.0five", -1, false);
		TryConvert("-5.0dfive", -1, false);
		TryConvert("5.0f", -1, false);
		TryConvert("-5.0f", -1, false);
		TryConvert("5.0d", -1, false);
	}

	private static void TryConvert<T>(string value, T expectedResult, bool expected)
	where T : IConvertible, IComparable<T>
	{
		Numeric.TryConvert(value, out T? result).ShouldBe(expected);

		if (expected) result.ShouldBe(expectedResult);
	}
	#endregion

	#region TestSizeToString
	[Test]
	public static void TestSizeToString()
	{
		SizeToString(0, 2, "0 B");
		SizeToString(1, 2, "1 B");
		SizeToString(1023, 2, "1023 B");
		SizeToString(1024, 2, "1.00 KiB");
		SizeToString(1025, 2, "1.00 KiB");
		SizeToString(1024 * 1024, 2, "1.00 MiB");
		SizeToString(1024 * 1024 * 1024, 2, "1.00 GiB");
		SizeToString(2137420969, 2, "1.99 GiB");
		SizeToString(2137420969, 6, "1.990628 GiB");
	}

	private static void SizeToString(long fileSize, int precision, string expected)
	{
		Numeric.SizeToString(fileSize, precision).ShouldBe(expected);
	}
	#endregion
}
