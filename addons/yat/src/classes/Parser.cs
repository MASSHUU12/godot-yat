using System;
using System.Collections.Generic;
using System.Linq;
using YAT.Enums;
using YAT.Helpers;
using YAT.Types;
using static YAT.Enums.ECommandInputType;

namespace YAT.Classes;

public static class Parser
{
	public static string[] ParseCommand(string command)
	{
		string[] tokens = Text.SanitizeText(command);

		return Text.ConcatenateSentence(tokens);
	}

	public static (string, string[]) ParseMethod(string method)
	{
		string[] tokens = Text.SanitizeText(method);
		List<string> args = new();

		if (tokens.Length == 0) return (string.Empty, Array.Empty<string>());

		var parts = (tokens.Length <= 1 ? tokens[^1] : tokens[0]).Split('(', 2,
			StringSplitOptions.RemoveEmptyEntries |
			StringSplitOptions.TrimEntries
		);

		if (parts.Length > 1 && !string.IsNullOrEmpty(parts[1][..^1]))
			args.Add(parts[1][..^1]);

		if (tokens.Length >= 1) for (int i = 1; i < tokens.Length; i++) args.Add(tokens[i][..^1]);

		return (parts[0], args.ToArray());
	}

	public static bool TryParseStringTypeToEnum(string type, out ECommandInputType parsed)
	{
		parsed = ECommandInputType.Void;

		if (System.Enum.TryParse(typeof(ECommandInputType), type, true, out var result))
		{
			parsed = (ECommandInputType)result;
			return true;
		}

		return false;
	}

	public static bool TryParseCommandInputType(string stringToParse, out CommandType parsed)
	{
		parsed = new();
		stringToParse = stringToParse.Trim();

		if (string.IsNullOrEmpty(stringToParse)) return false;

		bool isArray = stringToParse.EndsWith("...");
		if (isArray) stringToParse = stringToParse[..^3].Trim();

		// Get the min and max values if present
		var tokens = stringToParse.Trim(')').Split('(', StringSplitOptions.RemoveEmptyEntries);

		if (tokens.Length == 0 || tokens.Length > 2) return false;

		if ((!TryParseStringTypeToEnum(tokens[0], out var enumType) || enumType == Constant) && tokens.Length == 1)
		{
			parsed = new(Constant, isArray);
			return true;
		}

		if (tokens.Length > 1) // Type with range
		{
			if (!TryParseTypeWithRange(enumType, tokens[1], isArray, out var result))
				return false;

			parsed = result;
		}
		else parsed = new(enumType, isArray);

		return true;
	}

	public static bool TryParseTypeWithRange(ECommandInputType type, string range, bool isArray, out CommandTypeRanged parsed)
	{
		static bool AllowedToHaveRange(ECommandInputType type) =>
			type is Int or Float or ECommandInputType.String;

		CommandTypeRanged CreateOut(float min = float.MinValue, float max = float.MaxValue) =>
			new(type, isArray, min, max);

		parsed = new();

		if (!string.IsNullOrEmpty(range) && !AllowedToHaveRange(type))
			return false;

		// If range is missing
		if (string.IsNullOrEmpty(range))
		{
			parsed = CreateOut();

			return true;
		}

		ushort colonCount = (ushort)range.Count(c => c == ':');

		if (colonCount > 1) return false;

		bool maxPresent = !range.EndsWith(':');
		var minMax = range.Split(':', StringSplitOptions.RemoveEmptyEntries);

		// If range is invalid return
		if (minMax.Length == 0 || minMax.Length > 2) return false;

		// If only one value was passed (min or max)
		if (minMax.Length == 1)
		{
			if (!minMax[0].TryConvert(out float val)) return false;

			parsed = maxPresent
				? CreateOut(max: val)
				: CreateOut(min: val);

			return parsed.Min >= float.MinValue && parsed.Max <= float.MaxValue;
		}

		// If both values were passed
		if (minMax[0].TryConvert(out float min) && minMax[1].TryConvert(out float max))
		{
			parsed = CreateOut(min, max);
			return min < max;
		}

		return false;
	}
}
