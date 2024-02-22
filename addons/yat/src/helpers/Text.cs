using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YAT.Helpers;

public static class Text
{
	public static string EscapeBBCode(string text)
	{
		return text.Replace("[", "[lb]");
	}

	public static string MakeBold(string text)
	{
		return $"[b]{text}[/b]";
	}

	/// <summary>
	/// Sanitizes the given text by removing leading and trailing white spaces
	/// and empty entries.
	/// </summary>
	/// <param name="text">The text to sanitize.</param>
	/// <returns>An array of sanitized strings.</returns>
	public static string[] SanitizeText(string text)
	{
		return SplitClean(text, " ");
	}

	/// <summary>
	/// Concatenates sentences in an array of strings
	/// by removing quotation marks and
	/// merging consecutive strings enclosed in quotation marks.
	/// </summary>
	/// <param name="strings">The array of strings to concatenate.</param>
	/// <returns>An array of strings with concatenated sentences.</returns>
	public static string[] ConcatenateSentence(string[] strings)
	{
		List<string> modifiedStrings = new();

		for (ushort i = 0; i < strings.Length; i++)
		{
			string token = strings[i];
			bool startsWith = StartsWith(token, '"', '\'');

			// If the token starts and ends with quotation marks, remove them
			if (startsWith && EndsWith(token, '"', '\''))
			{
				modifiedStrings.Add(token.Length > 1 ? token[1..^1] : token[1..]);
				continue;
			}

			// Handle sentences in options (e.g. -name="John Doe")
			if (StartsWith(token, '-') &&
				(token.Contains('"') || token.Contains('\''))
				&& !EndsWith(token, '"', '\'')
			)
			{
				string sentence = token.Replace("\"", string.Empty).Replace("'", string.Empty);

				// Concatenate the next strings until the end of the sentence is reached
				while (!EndsWith(strings[i], '"', '\'') && i < strings.Length)
				{
					i++;
					if (i >= strings.Length) break;

					sentence += $" {strings[i]}";
				}

				sentence = i >= strings.Length ? sentence : sentence[..^1];
				modifiedStrings.Add(sentence);
				continue;
			}

			// If the token starts with a quotation mark, concatenate the next strings
			if (startsWith)
			{
				string sentence = strings[i][1..];

				// Concatenate the next strings until the end of the sentence is reached
				while (!EndsWith(strings[i], '"', '\'') && i < strings.Length)
				{
					i++;
					if (i >= strings.Length) break;
					sentence += $" {strings[i]}";
				}

				sentence = i >= strings.Length ? sentence : sentence[..^1];
				modifiedStrings.Add(sentence);
			}
			else modifiedStrings.Add(token);
		}

		return modifiedStrings.ToArray();
	}

	public static bool StartsWith(this string text, params char[] value)
	{
		return value.Any(text.StartsWith);
	}

	public static bool StartsWith(this string text, params string[] value)
	{
		return value.Any(text.StartsWith);
	}

	public static bool EndsWith(this string text, params char[] value)
	{
		return value.Any(text.EndsWith);
	}

	public static bool EndsWith(this string text, params string[] value)
	{
		return value.Any(text.EndsWith);
	}

	/// <summary>
	/// Splits the given text using the specified separator,
	/// trims each token, removes empty tokens,
	/// and returns an array of the resulting tokens.
	/// </summary>
	/// <param name="text">The text to split.</param>
	/// <param name="separator">The separator used to split the text.</param>
	/// <returns>An array of the resulting tokens.</returns>
	public static string[] SplitClean(string text, string separator)
	{
		return text.Split(separator, System.StringSplitOptions.TrimEntries |
								System.StringSplitOptions.RemoveEmptyEntries)
					.Select(token => token.Trim())
					.Where(token => !string.IsNullOrEmpty(token))
					.ToArray();
	}

	/// <summary>
	/// Shortens a file path to the specified length.
	///
	/// Adapted from https://stackoverflow.com/a/4638913
	/// </summary>
	/// <param name="path">The file path to shorten</param>
	/// <param name="maxLength">The max length of the output path (including the ellipsis if inserted)</param>
	/// <returns>
	/// The path with some of the middle directory paths replaced with an ellipsis
	/// (or the entire path if it is already shorter than maxLength)
	/// </returns>
	public static string ShortenPath(string path, ushort maxLength, string ellipsisChar = "...")
	{
		if (string.IsNullOrEmpty(path) || maxLength <= ellipsisChar.Length) return ellipsisChar;
		if (path.Length <= maxLength) return path;

		var dirSeparator = Path.DirectorySeparatorChar.ToString();
		bool isFirstPartsTurn = true;

		string[] pathParts = path.Split(dirSeparator);
		StringBuilder result = new();

		int totalLength = 0;
		int startIdx = 0;
		int endIdx = pathParts.Length - 1;

		while (startIdx <= endIdx && totalLength + ellipsisChar.Length <= maxLength)
		{
			var partToAdd = isFirstPartsTurn
				? pathParts[startIdx] + dirSeparator
				: dirSeparator + pathParts[endIdx];

			if (totalLength + partToAdd.Length > maxLength) break;

			if (isFirstPartsTurn)
			{
				result.Append(partToAdd);
				startIdx++;
			}
			else
			{
				result.Insert(0, partToAdd);
				endIdx--;
			}

			totalLength += partToAdd.Length;

			if (partToAdd != dirSeparator) isFirstPartsTurn = !isFirstPartsTurn;
		}

		string lastPart = string.Join(dirSeparator, pathParts, startIdx, endIdx - startIdx + 1);
		if (lastPart.Length + ellipsisChar.Length > maxLength)
		{
			lastPart = lastPart[^(maxLength - ellipsisChar.Length)..];
		}

		return result.ToString() + ellipsisChar + lastPart;
	}
}
