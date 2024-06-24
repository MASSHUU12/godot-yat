using YAT.Enums;

namespace YAT.Classes;

public class CommandTypeRanged : CommandType
{
	public float Min { get; private set; }
	public float Max { get; private set; }

	public CommandTypeRanged(ECommandInputType type, bool isArray, float min, float max) : base(type, isArray)
	{
		Min = min;
		Max = max;
	}
}
