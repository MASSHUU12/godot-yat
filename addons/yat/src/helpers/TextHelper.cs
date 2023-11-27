using System.Collections.Generic;
using System.Linq;

namespace YAT.Helpers
{
	/// <summary>
	/// Provides helper methods for working with text.
	/// </summary>
	public static class TextHelper
	{
		/// <summary>
		/// Escapes BBCode tags in the given text by replacing '[' with '[lb]'.
		/// </summary>
		/// <param name="text">The text to escape.</param>
		/// <returns>The escaped text.</returns>
		public static string EscapeBBCode(string text)
		{
			return text.Replace("[", "[lb]");
		}

		/// <summary>
		/// Makes the specified text bold by surrounding it with the appropriate tags.
		/// </summary>
		/// <param name="text">The text to make bold.</param>
		/// <returns>The specified text surrounded by bold tags.</returns>
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
			return text.Split(' ', System.StringSplitOptions.TrimEntries |
								System.StringSplitOptions.RemoveEmptyEntries)
						.Select(token => token.Trim())
						.Where(token => !string.IsNullOrEmpty(token))
						.ToArray();
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
				bool startsWith = token.IndexOfAny(new[] { '"', '\'' }) == 0;

				// If the token starts and ends with quotation marks, remove them
				if (startsWith && EndsWith(token, '"', '\''))
				{
					modifiedStrings.Add(token.Length > 1 ? token[1..^1] : token[1..]);
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

		/// <summary>
		/// Determines whether the specified text ends with any of the specified characters.
		/// </summary>
		/// <param name="text">The text to check.</param>
		/// <param name="value">The characters to compare against the end of the text.</param>
		/// <returns><c>true</c> if the text ends with any of the specified characters; otherwise, <c>false</c>.</returns>
		public static bool EndsWith(string text, params char[] value)
		{
			return value.Any(text.EndsWith);
		}
	}
}
