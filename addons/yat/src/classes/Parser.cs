using System;
using System.Collections.Generic;
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
		parsed = null;
		type = type?.Trim();

		// Used to later get min/max values
		static bool allowedToHaveRange(string t) => t switch
		{
			"int" => true,
			"float" => true,
			"string" => true,
			_ => false
		};

		if (string.IsNullOrEmpty(type)) return false;

		// Check if ends with ... to indicate an array
		bool isArray = type.EndsWith("...");

		if (isArray) type = type[..^3].Trim();

		// Get the min and max values if present
		var tokens = type.Trim(')').Split('(', StringSplitOptions.RemoveEmptyEntries);

		if (tokens.Length > 1)
		{
			bool maxIsPresent = tokens[1].EndsWith(':');
			var minMax = tokens[1].Split(':', StringSplitOptions.RemoveEmptyEntries);

			if (!allowedToHaveRange(tokens[0]) || minMax.Length > 2) return false;

			if (minMax.Length == 1)
			{
				// If only min value is not present, set it to float.MinValue
				if (!maxIsPresent)
				{
					if (minMax[0].TryConvert(out float max))
						parsed = new(tokens[0], float.MinValue, max, isArray);
					else return false;
				} // If only max value is not present, set it to float.MaxValue
				else
				{
					if (minMax[0].TryConvert(out float min))
						parsed = new(tokens[0], min, float.MaxValue, isArray);
					else return false;
				}
			}
			else
			{
				if (minMax[0].TryConvert(out float min)
					&& minMax[1].TryConvert(out float max)
				) parsed = new(tokens[0], min, max, isArray);
				else return false;
			}
		}
		else parsed = new(tokens[0], 0, 0, isArray);

		if (parsed.Min > parsed.Max) return false;

		return true;
	}
}
