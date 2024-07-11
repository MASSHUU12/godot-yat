using System;
using System.ComponentModel;

namespace YAT.Helpers;

public static class Numeric
{
    public static bool IsWithinRange<T>(this T value, T min, T max)
    where T : IComparable<T>
    {
        return value.IsWithinRange<T, T>(min, max);
    }

    public static bool IsWithinRange<T, E>(this T value, E min, E max)
    where T : IComparable<T>, IComparable<E>
    {
        return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
    }

    /// <summary>
    /// Tries to convert a string to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to convert the string to.</typeparam>
    /// <param name="input">The string to convert.</param>
    /// <param name="output">When this method returns, contains the converted value if the conversion succeeded, or the fallback value if the conversion failed.</param>
    /// <param name="fallback">The fallback value to use if the conversion fails. The default value of the type T is used if no fallback value is specified.</param>
    /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
    public static bool TryConvert<T>(this string input, out T? output, T? fallback = default)
        where T : notnull, IConvertible, IComparable<T>
    {
        output = fallback;

        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        TypeConverter? converter = TypeDescriptor.GetConverter(typeof(T));

        if (converter?.CanConvertFrom(typeof(string)) != true)
        {
            return false;
        }

        try
        {
            output = (T?)converter.ConvertFrom(input);
            return output is not null;
        }
        catch (Exception ex) when (ex
            is InvalidCastException
            or NotSupportedException
            or FormatException
            or ArgumentException
        )
        { return false; }
    }

    /// <summary>
    /// Converts a size in bytes to a human-readable string representation.
    /// </summary>
    /// <param name="fileSize">The file size in bytes.</param>
    /// <param name="precision">The number of decimal places to use.</param>
    /// <returns>A string representing the file size in a human-readable format.</returns>
    public static string SizeToString(long fileSize, int precision = 2)
    {
        const int byteConversion = 1024;
        double bytes = fileSize;

        if (bytes < byteConversion)
        {
            return $"{bytes} B";
        }

        double kilobytes = bytes / byteConversion;
        if (kilobytes < byteConversion)
        {
            return $"{kilobytes.ToString($"F{precision}")} KiB";
        }

        double megabytes = kilobytes / byteConversion;
        if (megabytes < byteConversion)
        {
            return $"{megabytes.ToString($"F{precision}")} MiB";
        }

        double gigabytes = megabytes / byteConversion;

        return $"{gigabytes.ToString($"F{precision}")} GiB";
    }
}
