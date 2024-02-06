using System;
using System.Collections.Generic;
using YAT.Helpers;

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
}
