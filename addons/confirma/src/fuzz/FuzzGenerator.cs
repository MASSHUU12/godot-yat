using System;
using Confirma.Extensions;

using static Confirma.Fuzz.EDistributionType;

namespace Confirma.Fuzz;

public class FuzzGenerator(
    Type dataType,
    string? name,
    double min,
    double max,
    double mean,
    double standardDeviation,
    double lambda,
    EDistributionType distribution,
    int? seed
    )
{
    public Type DataType { get; init; } = dataType;
    public string? Name { get; init; } = name;
    public double Min { get; init; } = min;
    public double Max { get; init; } = max;
    public double Mean { get; init; } = mean;
    public double StandardDeviation { get; init; } = standardDeviation;
    public double Lambda { get; init; } = lambda;
    public EDistributionType Distribution { get; init; } = distribution;
    public int? Seed { get; init; } = seed;

    private readonly Random _rg = seed.HasValue ? new(seed.Value) : new();

    public object NextValue()
    {
        return DataType switch
        {
            Type t when t == typeof(int) => NextInt(),
            Type t when t == typeof(double) => NextDouble(),
            Type t when t == typeof(float) => (float)NextDouble(),
            Type t when t == typeof(string) => NextString(),
            Type t when t == typeof(bool) => _rg.NextBool(),
            _ => throw new ArgumentException($"{DataType.Name} is unsupported."),
        };
    }

    private int NextInt()
    {
        return Distribution switch
        {
            Gaussian => (int)_rg.NextGaussianDouble(
                Mean,
                StandardDeviation
            ),
            Exponential => (int)_rg.NextExponentialDouble(Lambda),
            Poisson => _rg.NextPoissonInt(Lambda),
            Uniform or _ => (int)_rg.NextInt64((int)Min, (int)Max)
        };
    }

    private double NextDouble()
    {
        return Distribution switch
        {
            Gaussian => _rg.NextGaussianDouble(
                Mean,
                StandardDeviation
            ),
            Exponential => _rg.NextExponentialDouble(Lambda),
            Poisson => _rg.NextPoissonInt(Lambda),
            Uniform or _ => _rg.NextDouble(Min, Max)
        };
    }

    private string NextString()
    {
        return _rg.NextString((int)Min, (int)Max);
    }
}
