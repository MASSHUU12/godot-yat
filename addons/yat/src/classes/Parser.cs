using System;
using System.Collections.Generic;
using System.Linq;
using YAT.Helpers;
using YAT.Types;

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

	public static bool TryParseCommandInputType(string type, out CommandInputType parsed)
	{
		parsed = new();
		type = type.Trim();

		if (string.IsNullOrEmpty(type)) return false;

		bool isArray = type.EndsWith("...");
		if (isArray) type = type[..^3].Trim();

		// Get the min and max values if present
		var tokens = type.Trim(')').Split('(', StringSplitOptions.RemoveEmptyEntries);

		if (tokens.Length == 0 || tokens.Length > 2) return false;

		if (tokens.Length > 1) // type with range
		{
			if (!TryParseTypeWithRange(tokens[0], tokens[1], isArray, out parsed))
				return false;
		}
		// Check if only range was passed
		else if (!tokens[0].Contains(':'))
			parsed = new(tokens[0], float.MinValue, float.MaxValue, isArray);
		else return false;

		return parsed.Min < parsed.Max;
	}

	public static bool TryParseTypeWithRange(string type, string range, bool isArray, out CommandInputType parsed)
	{
		static bool AllowedToHaveRange(string type) =>
			type is "int" or "float" or "string";

		CommandInputType CreateOut(float min = float.MinValue, float max = float.MaxValue) =>
			new(type, min, max, isArray);

		parsed = new();

		if (string.IsNullOrEmpty(type) || (!string.IsNullOrEmpty(range) && !AllowedToHaveRange(type)))
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

			return true;
		}

		// If both values were passed
		if (minMax[0].TryConvert(out float min) && minMax[1].TryConvert(out float max))
		{
			parsed = CreateOut(min, max);
			return true;
		}

		return false;
	}
}
