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
        return rg.NextNonNegativeDecimal() / decimal.MaxValue * maxValue;
    }

    public static decimal NextDecimal(
        this Random rg,
        decimal minValue,
        decimal maxValue
    )
    {
        return minValue > maxValue
            ? throw new InvalidOperationException()
            : rg.NextDecimal(maxValue - minValue) + minValue;
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
        return minValue > maxValue
            ? throw new InvalidOperationException()
            : rg.NextLong(maxValue - minValue) + minValue;
    }

    public static double NextDouble(this Random rg, double minValue, double maxValue)
    {
        return minValue > maxValue
            ? throw new InvalidOperationException()
            : (rg.NextDouble() * (maxValue - minValue)) + minValue;
    }

    public static double NextGaussianDouble(
        this Random rg,
        double mean,
        double standardDeviation
    )
    {
        double u1 = rg.NextDouble();
        double u2 = rg.NextDouble();
        double standardNormal = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
        return mean + (standardDeviation * standardNormal);
    }

    public static double NextExponentialDouble(this Random rg, double lambda)
    {
        return -Math.Log(rg.NextDouble()) / lambda;
    }

    public static int NextPoissonInt(this Random rg, double lambda)
    {
        if (lambda == 0.0)
        {
            return 0;
        }

        double l = Math.Exp(-lambda);
        int k = 0;
        double p = 1.0;

        while (p > l)
        {
            p *= rg.NextDouble();
            k++;
        }

        return k - 1;
    }
}
