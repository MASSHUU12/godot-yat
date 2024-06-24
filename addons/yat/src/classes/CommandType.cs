using YAT.Enums;

namespace YAT.Classes;

public class CommandType
{
	public ECommandInputType Type { get; private set; }
	public bool IsArray { get; private set; }

	public CommandType(ECommandInputType type, bool isArray)
	{
		Type = type;
		IsArray = isArray;
	}
}
