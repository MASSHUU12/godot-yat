using YAT.Enums;

namespace YAT.Classes;

public class CommandType
{
    public string Name { get; init; } = string.Empty;
    public ECommandInputType Type { get; init; } = ECommandInputType.Void;
    public bool IsArray { get; init; }
    public string TypeDefinition { get; protected set; }

    public CommandType(string name, ECommandInputType type, bool isArray)
    {
        Name = name;
        Type = type;
        IsArray = isArray;
        TypeDefinition = Name;
    }

    public CommandType()
    {
        TypeDefinition = GenerateTypeDefinition();
    }

    protected virtual string GenerateTypeDefinition() => Name;
}
