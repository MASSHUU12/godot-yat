using Confirma.Interfaces;

namespace Confirma.Formatters;

public abstract class Formatter : IFormatter
{
    public static readonly string DefaultFormat = "Expected {1} but was {2}.";
    public abstract string Format(object? value);
}
