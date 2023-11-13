using System;
using System.ComponentModel;

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

		/// <summary>
		/// Tries to convert the specified string representation of a number to its type T equivalent.
		/// </summary>
		/// <typeparam name="T">The type of the output.</typeparam>
		/// <param name="input">The string to convert.</param>
		/// <param name="output">When this method returns, contains the converted value if the conversion succeeded, or the default value of T if the conversion failed.</param>
		/// <returns>true if the conversion succeeded; otherwise, false.</returns>
		public static bool TryConvert<T>(this string input, out T output) where T : IConvertible, IComparable
		{
			var converter = TypeDescriptor.GetConverter(typeof(T));

			if (converter.CanConvertFrom(typeof(string)))
			{
				try
				{
					output = (T)converter.ConvertFrom(input);
					return true;
				}
				catch (Exception)
				{
					output = default;
					return false;
				}
			}

			output = default;
			return false;
		}
	}
}
