using System;
using System.Collections.Generic;
using System.Linq;

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
                    if (i >= strings.Length)
                    {
                        break;
                    }

                    sentence += $" {strings[i]}";
                }

                sentence = i >= strings.Length ? sentence : sentence[..^1];
                modifiedStrings.Add(sentence);
            }
            else
            {
                modifiedStrings.Add(token);
            }
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
    /// <para>Shortens a file path to the specified length.</para>
    /// <para>Adapted from https://stackoverflow.com/a/32664181</para>
    /// </summary>
    /// <param name="path"></param>
    /// <param name="maxLength"></param>
    /// <param name="ellipsisChar"></param>
    /// <param name="splitChar"></param>
    public static string ShortenPath(string path, ushort maxLength, string ellipsisChar = "...", string splitChar = "/")
    {
        if (string.IsNullOrEmpty(path) || maxLength <= ellipsisChar.Length)
        {
            return ellipsisChar;
        }

        if (path.Length <= maxLength)
        {
            return path;
        }

        string[] parts = path.Split(splitChar);

        int centerIndex = (parts.Length - 1) >> 1,
            currentIndex = centerIndex,
            lean = 1;
        decimal step = 0;
        string output = string.Join(splitChar, parts, 0, parts.Length);

        while (output.Length >= maxLength && currentIndex != 0 && currentIndex != -1)
        {
            parts[currentIndex] = ellipsisChar;

            output = string.Join(splitChar, parts, 0, parts.Length);

            step += 0.5M;
            lean *= -1;

            currentIndex = centerIndex + ((int)step * lean);
        }

        // Ensure the result does not exceed maxLength
        return output[..Math.Min(maxLength, output.Length)];
    }
}
