using YAT.Enums;

namespace YAT.Classes;

public class CommandType
{
	public string Name { get; init; } = string.Empty;
	public ECommandInputType Type { get; init; } = ECommandInputType.Void;
	public bool IsArray { get; init; } = false;

	public CommandType(string name, ECommandInputType type, bool isArray)
	{
		Name = name;
		Type = type;
		IsArray = isArray;
	}

	public CommandType() { }

	public virtual string GetTypeDefinition()
	{
		return Name;
	}
}
