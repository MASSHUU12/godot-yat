using System;

namespace Confirma.Extensions;

/// <summary>
/// Inspired by https://stackoverflow.com/a/2000459.
/// </summary>
public static class RandomNumberExtensions
{
    public static int NextInt32(this Random rg)
    {
        unchecked
        {
            int firstBits = rg.Next(0, 1 << 4) << 28;
            int lastBits = rg.Next(0, 1 << 28);
            return firstBits | lastBits;
        }
    }

    public static int NextDigit(this Random rg)
    {
        return rg.Next(0, 10);
    }

    public static int NextNonZeroDigit(this Random rg)
    {
        return rg.Next(1, 10);
    }

    public static decimal NextDecimal(this Random rg)
    {
        return rg.NextDecimal(rg.Next(2) == 1);
    }

    public static decimal NextDecimal(this Random rg, bool sign)
    {
        return new(
            rg.NextInt32(),
            rg.NextInt32(),
            rg.NextInt32(),
            sign,
            (byte)rg.Next(29)
        );
    }

    public static decimal NextNonNegativeDecimal(this Random rg)
    {
        return rg.NextDecimal(false);
    }

    public static decimal NextDecimal(this Random rg, decimal maxValue)
    {
        return rg.NextNonNegativeDecimal() / decimal.MaxValue * maxValue; ;
    }

    public static decimal NextDecimal(this Random rg, decimal minValue, decimal maxValue)
    {
        if (minValue >= maxValue) throw new InvalidOperationException();

        return rg.NextDecimal(maxValue - minValue) + minValue;
    }

    public static long NextNonNegativeLong(this Random rg)
    {
        byte[] bytes = new byte[sizeof(long)];
        rg.NextBytes(bytes);
        // strip out the sign bit
        bytes[7] = (byte)(bytes[7] & 0x7f);
        return BitConverter.ToInt64(bytes, 0);
    }

    public static long NextLong(this Random rg, long maxValue)
    {
        return (long)(rg.NextNonNegativeLong() / (double)long.MaxValue * maxValue);
    }

    public static long NextLong(this Random rg, long minValue, long maxValue)
    {
        if (minValue >= maxValue) throw new InvalidOperationException();

        return rg.NextLong(maxValue - minValue) + minValue;
    }

    public static double NextDouble(this Random rg, double minValue, double maxValue)
    {
        if (minValue >= maxValue) throw new InvalidOperationException();

        return rg.NextDouble() * (maxValue - minValue) + minValue;
    }
}
