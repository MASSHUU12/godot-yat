using System;

namespace YAT.Helpers
{
	public static class NumericHelper
	{
		/// <summary>
		/// Determines whether a value is within the specified range (inclusive).
		/// </summary>
		/// <typeparam name="T">The type of the values to compare.</typeparam>
		/// <param name="value">The value to check.</param>
		/// <param name="min">The minimum value of the range.</param>
		/// <param name="max">The maximum value of the range.</param>
		/// <returns>True if the value is within the range, false otherwise.</returns>
		public static bool IsWithinRange<T>(T value, T min, T max) where T : IComparable<T>
		{
			return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
		}
	}
}
