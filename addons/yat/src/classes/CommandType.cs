using YAT.Enums;

namespace YAT.Classes;

public class CommandType
{
	public string Name { get; private set; } = string.Empty;
	public ECommandInputType Type { get; private set; } = ECommandInputType.Void;
	public bool IsArray { get; private set; } = false;

	public CommandType(string name, ECommandInputType type, bool isArray)
	{
		Name = name;
		Type = type;
		IsArray = isArray;
	}

	public CommandType() { }
}
