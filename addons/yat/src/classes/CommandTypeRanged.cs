using YAT.Enums;

namespace YAT.Classes;

public class CommandTypeRanged : CommandType
{
	public float Min { get; private set; } = float.MinValue;
	public float Max { get; private set; } = float.MaxValue;

	public CommandTypeRanged(string name, ECommandInputType type, bool isArray, float min, float max)
	: base(name, type, isArray)
	{
		Min = min;
		Max = max;
	}

	public CommandTypeRanged() : base() { }
}
