using System.Collections.Generic;
using YAT.Enums;

namespace YAT.Classes;

public class CommandTypeEnum : CommandType
{
	public Dictionary<string, int> Values { get; private set; }

	public CommandTypeEnum(string name, ECommandInputType type, bool isArray, Dictionary<string, int> values)
	: base(name, type, isArray)
	{
		Values = values;
	}
}
