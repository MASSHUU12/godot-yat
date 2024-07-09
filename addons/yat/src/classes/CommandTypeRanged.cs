using YAT.Enums;

namespace YAT.Classes;

public class CommandTypeRanged : CommandType
{
    public readonly float Min = float.MinValue;
    public readonly float Max = float.MaxValue;

    public CommandTypeRanged(string name, ECommandInputType type, bool isArray, float min, float max)
    : base(name, type, isArray)
    {
        Min = min;
        Max = max;
        TypeDefinition = GenerateTypeDefinition();
    }

    public CommandTypeRanged(string name, ECommandInputType type, bool isArray)
    : base(name, type, isArray)
    {
        TypeDefinition = GenerateTypeDefinition();
    }

    public CommandTypeRanged() : base() { }

    protected override string GenerateTypeDefinition()
    {
        return $"{Name}({(
            Min == float.MinValue ? string.Empty : Min
        )}:{(
            Max == float.MaxValue ? string.Empty : Max
        )})";
    }
}
