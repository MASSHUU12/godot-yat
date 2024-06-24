using System.Collections.Generic;
using YAT.Enums;

namespace YAT.Classes;

public class CommandTypeEnum : CommandType
{
	public Dictionary<string, int> Values { get; private set; }

	public CommandTypeEnum(ECommandInputType type, bool isArray, Dictionary<string, int> values)
	: base(type, isArray)
	{
		Values = values;
	}
}
