using YAT.Enums;

namespace YAT.Classes;

public class CommandTypeRanged : CommandType
{
	public float Min { get; init; } = float.MinValue;
	public float Max { get; init; } = float.MaxValue;

	public CommandTypeRanged(string name, ECommandInputType type, bool isArray, float min, float max)
	: base(name, type, isArray)
	{
		Min = min;
		Max = max;
	}

	public CommandTypeRanged() : base() { }

	public override string GetTypeDefinition()
	{
		return $"{Name}({(
			Min == float.MinValue ? string.Empty : Min
		)}:{(
			Max == float.MaxValue ? string.Empty : Max
		)})";
	}
}
