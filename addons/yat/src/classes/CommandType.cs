using YAT.Enums;

namespace YAT.Classes;

public class CommandType
{
	public ECommandInputType Type { get; private set; } = ECommandInputType.Void;
	public bool IsArray { get; private set; } = false;

	public CommandType(ECommandInputType type, bool isArray)
	{
		Type = type;
		IsArray = isArray;
	}

	public CommandType() { }
}
