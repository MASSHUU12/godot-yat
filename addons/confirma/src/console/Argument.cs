using System;

using static Confirma.Terminal.EArgumentParseResult;

namespace Confirma.Terminal;

// TODO: Implement a fluent interface for registering arguments e.q.:
// new Argument("verbose").WithAlias("v").WithDescription("Enable verbose output");
public class Argument(
    string name,
    bool usePrefix = true,
    bool isFlag = false,
    bool allowEmpty = false,
    Action<object>? action = null
    )
{
    public string Name { get; init; } = name;

    public bool IsFlag { get; init; } = isFlag;
    public bool UsePrefix { get; init; } = usePrefix;
    public bool AllowEmpty { get; init; } = allowEmpty;

    private readonly Action<object>? _action = action;

    public EArgumentParseResult Parse(string? value, out object? parsed)
    {
        parsed = null;

        if (IsFlag)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return UnexpectedValue;
            }
            parsed = true;
        }
        else
        {
            if (!AllowEmpty && string.IsNullOrEmpty(value))
            {
                return ValueRequired;
            }

            parsed = value;
        }

        return Success;
    }

    public void Invoke(object value)
    {
        _action?.Invoke(value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Argument other
            && Name == other.Name
            && UsePrefix == other.UsePrefix
            && IsFlag == other.IsFlag
            && AllowEmpty == other.AllowEmpty;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, UsePrefix);
    }
}
