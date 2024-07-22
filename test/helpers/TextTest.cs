using System.Text.RegularExpressions;
using Confirma.Attributes;
using Confirma.Classes;
using Confirma.Extensions;
using YAT.Helpers;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class TextTest
{
    [TestCase("Hello, world!", "Hello, world!")]
    [TestCase("Hello, [b]world[/b]!", "Hello, [lb]b]world[lb]/b]!")]
    [TestCase("[b]Hello[/b], world!", "[lb]b]Hello[lb]/b], world!")]
    [TestCase("[b]Hello, world![/b]", "[lb]b]Hello, world![lb]/b]")]
    [TestCase("[b]Hello, [b]world[/b]![/b]", "[lb]b]Hello, [lb]b]world[lb]/b]![lb]/b]")]
    public static void EscapeBBCode(string text, string expected)
    {
        _ = Text.EscapeBBCode(text).ConfirmEqual(expected);
    }

    [TestCase("Hello, world!", "[b]Hello, world![/b]")]
    [TestCase("Hello, [b]world[/b]!", "[b]Hello, [b]world[/b]![/b]")]
    [TestCase("[b]Hello[/b], world!", "[b][b]Hello[/b], world![/b]")]
    [TestCase("[b]Hello, world![/b]", "[b][b]Hello, world![/b][/b]")]
    [TestCase("[b]Hello, [b]world[/b]![/b]", "[b][b]Hello, [b]world[/b]![/b][/b]")]
    public static void MakeBold(string text, string expected)
    {
        _ = Text.MakeBold(text).ConfirmEqual(expected);
    }

    [TestCase("Hello, world!", new string[] { "Hello,", "world!" })]
    [TestCase("Hello, world! ", new string[] { "Hello,", "world!" })]
    [TestCase(" Hello, world!", new string[] { "Hello,", "world!" })]
    [TestCase(" Hello, world! ", new string[] { "Hello,", "world!" })]
    [TestCase("Hello,  world!", new string[] { "Hello,", "world!" })]
    [TestCase("  Hello,  world!    ", new string[] { "Hello,", "world!" })]
    public static void SanitizeText(string text, string[] expected)
    {
        _ = Text.SanitizeText(text).ConfirmEqual(expected);
    }

    [TestCase(new string[] { "Hello,", "world!" }, new string[] { "Hello,", "world!" })]
    [TestCase(new string[] { "echo", "\"Hello,", "world!\"" }, new string[] { "echo", "Hello, world!" })]
    [TestCase(new string[] { "echo", "'Hello,", "world!'" }, new string[] { "echo", "Hello, world!" })]
    [TestCase(new string[] { "qc", "add", "-name=\"John", "Doe\"" }, new string[] { "qc", "add", "-name=John Doe" })]
    [TestCase(new string[] { "qc", "add", "-name='John", "Doe'" }, new string[] { "qc", "add", "-name=John Doe" })]
    [TestCase(new string[] { "qc", "add", "-name='John", "\"Doe'" }, new string[] { "qc", "add", "-name=John \"Doe" })]
    public static void ConcatenateSentence(string[] strings, string[] expected)
    {
        _ = Text.ConcatenateSentence(strings).ConfirmEqual(expected);
    }

    [TestCase("Hello, world!", true, new char[] { 'H', 'H', 'H' })]
    [TestCase("Hello, world!", true, new char[] { 'H', 'h' })]
    [TestCase("Hello, world!", true, new char[] { 'H', 'e' })]
    [TestCase("Hello, world!", true, new char[] { 'H', 'l' })]
    [TestCase("Hello, world!", true, new char[] { 'o', 'H' })]
    [TestCase("Hello, world!", false, new char[] { 'o', 'o' })]
    public static void StartsWith(string text, bool expected, char[] chars)
    {
        _ = text.StartsWith(chars).ConfirmEqual(expected);
    }

    [TestCase("Hello, world!", true, new char[] { '!', '!', '!' })]
    [TestCase("Hello, world!", true, new char[] { '!', 'd' })]
    [TestCase("Hello, world!", true, new char[] { '!', 'l' })]
    [TestCase("Hello, world!", true, new char[] { '!', 'r' })]
    [TestCase("Hello, world!", true, new char[] { 'w', '!' })]
    public static void EndsWith(string text, bool expected, char[] chars)
    {
        _ = text.EndsWith(chars).ConfirmEqual(expected);
    }

    [TestCase("Hello!", "", new string[] { "Hello!" })]
    [TestCase("Hello!", " ", new string[] { "Hello!" })]
    [TestCase("Hello!", "l", new string[] { "He", "o!" })]
    [TestCase("Hello!", "ll", new string[] { "He", "o!" })]
    [TestCase("Hello, World!", ", ", new string[] { "Hello", "World!" })]
    public static void SplitClean(string text, string separator, string[] expected)
    {
        _ = Text.SplitClean(text, separator).ConfirmEqual(expected);
    }

    [TestCase("", 16u, "...")]
    [TestCase("res://example/main_menu/MainMenu.tscn", 0u, "...")]
    [TestCase("res://example/main_menu/MainMenu.tscn", 15u, "res:/.../.../..")]
    [TestCase("res://example/main_menu/MainMenu.tscn", 30u, "res://.../.../MainMenu.tscn")]
    [TestCase("res://example/main_menu/MainMenu.tscn", 128u, "res://example/main_menu/MainMenu.tscn")]
    public static void ShortenPath(string path, uint length, string expected)
    {
        _ = Text.ShortenPath(path, (ushort)length).ConfirmEqual(expected);
    }

    #region WildcardToRegex
    [TestCase]
    public static void WildcardToRegex_SimpleWildcard()
    {
        string regex = Text.WildcardToRegex("abc*def");

        _ = Confirm.IsTrue(Regex.IsMatch("abcdef", regex));
        _ = Confirm.IsTrue(Regex.IsMatch("abc123def", regex));
        _ = Confirm.IsTrue(Regex.IsMatch("abcddef", regex));

        _ = Confirm.IsFalse(Regex.IsMatch(string.Empty, regex));
        _ = Confirm.IsFalse(Regex.IsMatch("def", regex));
    }

    [TestCase]
    public static void WildcardToRegex_QuestionMarkWildcard()
    {
        string regex = Text.WildcardToRegex("abc?def");

        _ = Confirm.IsTrue(Regex.IsMatch("abcddef", regex));
        _ = Confirm.IsTrue(Regex.IsMatch("abcxdef", regex));
        _ = Confirm.IsFalse(Regex.IsMatch("abc123def", regex));
        _ = Confirm.IsFalse(Regex.IsMatch(string.Empty, regex));
    }

    [TestCase]
    public static void WildcardToRegex_MultipleWildcards()
    {
        string regex = Text.WildcardToRegex("abc*def?ghi");

        _ = Confirm.IsTrue(Regex.IsMatch("abcdef1ghi", regex));
        _ = Confirm.IsTrue(Regex.IsMatch("abc123defxghi", regex));
        _ = Confirm.IsFalse(Regex.IsMatch("abcddefghi", regex));
        _ = Confirm.IsFalse(Regex.IsMatch(string.Empty, regex));
    }

    [TestCase]
    public static void WildcardToRegex_NoWildcards()
    {
        string regex = Text.WildcardToRegex("abcdef");

        _ = Confirm.IsTrue(Regex.IsMatch("abcdef", regex));

        _ = Confirm.IsFalse(Regex.IsMatch("abcddef", regex));
        _ = Confirm.IsFalse(Regex.IsMatch(string.Empty, regex));
    }

    [TestCase]
    public static void WildcardToRegex_EmptyWildcard()
    {
        string regex = Text.WildcardToRegex(string.Empty);

        _ = Confirm.IsTrue(Regex.IsMatch(string.Empty, regex));
        _ = Confirm.IsFalse(Regex.IsMatch("d", regex));
    }
    #endregion WildcardToRegex
}
