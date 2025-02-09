using System;
using Confirma.Attributes;
using Confirma.Classes;
using Confirma.Extensions;
using YAT.Helpers;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public class TextTest
{
    #region EscapeBBCode
    [TestCase("Hello, world!", "Hello, world!")]
    [TestCase("Hello, [b]world[/b]!", "Hello, [lb]b]world[lb]/b]!")]
    [TestCase("[b]Hello[/b], world!", "[lb]b]Hello[lb]/b], world!")]
    [TestCase("[b]Hello, world![/b]", "[lb]b]Hello, world![lb]/b]")]
    [TestCase("[b]Hello, [b]world[/b]![/b]", "[lb]b]Hello, [lb]b]world[lb]/b]![lb]/b]")]
    public void EscapeBBCode_ReturnsEscapedText(
        string text,
        string expected
    )
    {
        _ = Text.EscapeBBCode(text).ConfirmEqual(expected);
    }
    #endregion EscapeBBCode

    #region MakeBold
    [TestCase("Hello, world!", "[b]Hello, world![/b]")]
    [TestCase("Hello, [b]world[/b]!", "[b]Hello, [b]world[/b]![/b]")]
    [TestCase("[b]Hello[/b], world!", "[b][b]Hello[/b], world![/b]")]
    [TestCase("[i]Hello, world![/i]", "[b][i]Hello, world![/i][/b]")]
    [TestCase("[b]Hello, [b]world[/b]![/b]", "[b][b]Hello, [b]world[/b]![/b][/b]")]
    public void MakeBold_WrapsTextInBTag(string text, string expected)
    {
        _ = Text.MakeBold(text).ConfirmEqual(expected);
    }
    #endregion MakeBold

    #region SanitizeText
    [TestCase("Hello, world!", new string[] { "Hello,", "world!" })]
    [TestCase("Hello, world! ", new string[] { "Hello,", "world!" })]
    [TestCase(" Hello, world!", new string[] { "Hello,", "world!" })]
    [TestCase(" Hello, world! ", new string[] { "Hello,", "world!" })]
    [TestCase("Hello,  world!", new string[] { "Hello,", "world!" })]
    [TestCase("  Hello,  world!    ", new string[] { "Hello,", "world!" })]
    public void SanitizeText_SanitizesAndSplitsText(
        string text,
        string[] expected
    )
    {
        _ = Text.SanitizeText(text).ConfirmEqual(expected);
    }
    #endregion SanitizeText

    #region ConcatenateSentence
    [TestCase(
        new string[] { "Hello,", "world!" },
        new string[] { "Hello,", "world!" }
    )]
    [TestCase(
        new string[] { "echo", "\"Hello,", "world!\"" },
        new string[] { "echo", "Hello, world!" }
    )]
    [TestCase(
        new string[] { "echo", "'Hello,", "world!'" },
        new string[] { "echo", "Hello, world!" }
    )]
    [TestCase(
        new string[] { "qc", "add", "-name=\"John", "Doe\"" },
        new string[] { "qc", "add", "-name=John Doe" }
    )]
    [TestCase(
        new string[] { "qc", "add", "-name='John", "Doe'" },
        new string[] { "qc", "add", "-name=John Doe" }
    )]
    [TestCase(
        new string[] { "qc", "add", "-name='John", "\"Doe'" },
        new string[] { "qc", "add", "-name=John \"Doe" }
    )]
    public void ConcatenateSentence_ConcatenatesSentences(
        string[] strings,
        string[] expected
    )
    {
        _ = Text.ConcatenateSentence(strings).ConfirmEqual(expected);
    }
    #endregion ConcatenateSentence

    #region StartsWith
    [TestCase("Hello, world!", new char[] { 'H', 'H', 'H' })]
    [TestCase("Hello, world!", new char[] { 'H', 'h' })]
    [TestCase("Hello, world!", new char[] { 'e', 'H' })]
    [TestCase("Hello, world!", new char[] { 'H', 'l' })]
    [TestCase("Hello, world!", new char[] { 'o', 'H' })]
    public void StartsWith_WhenStartsWith(string text, char[] chars)
    {
        _ = text.StartsWith(chars).ConfirmEqual(true);
    }

    [TestCase("Hello, world!", new char[] { 'o', 'o' })]
    [TestCase("Hello, world!", new char[] { 'h' })]
    public void StartsWith_WhenNotStartsWith(string text, char[] chars)
    {
        _ = text.StartsWith(chars).ConfirmEqual(false);
    }
    #endregion StartsWith

    #region EndsWith
    [TestCase("Hello, world!", new char[] { '!', '!', '!' })]
    [TestCase("Hello, world!", new char[] { '!', 'd' })]
    [TestCase("Hello, world!", new char[] { '!', 'l' })]
    [TestCase("Hello, world!", new char[] { '!', 'r' })]
    [TestCase("Hello, world!", new char[] { 'w', '!' })]
    public void EndsWith_WhenEndsWith(string text, char[] chars)
    {
        _ = text.EndsWith(chars).ConfirmEqual(true);
    }

    [TestCase("Hello, world!", new char[] { 'd' })]
    [TestCase("Hello, world!", new char[] { 'H', 'h' })]
    public void EndsWith_WhenNotEndsWith(string text, char[] chars)
    {
        _ = text.EndsWith(chars).ConfirmEqual(false);
    }
    #endregion EndsWith

    #region SplitClean
    [TestCase("Hello!", "", new string[] { "Hello!" })]
    [TestCase("Hello!", " ", new string[] { "Hello!" })]
    [TestCase("Hello!", "l", new string[] { "He", "o!" })]
    [TestCase("Hello!", "ll", new string[] { "He", "o!" })]
    [TestCase("Hello, World!", ", ", new string[] { "Hello", "World!" })]
    public void SplitClean_SplitsStringCorrectly(
        string text,
        string separator,
        string[] expected
    )
    {
        _ = Text.SplitClean(text, separator).ConfirmEqual(expected);
    }
    #endregion SplitClean

    #region ShortenPath
    [TestCase("", 16u, "...")]
    [TestCase("res://example/main_menu/MainMenu.tscn", 0u, "...")]
    [TestCase("res://example/main_menu/MainMenu.tscn", 15u, "res:/.../.../..")]
    [TestCase(
        "res://example/main_menu/MainMenu.tscn",
        30u,
        "res://.../.../MainMenu.tscn"
    )]
    [TestCase(
        "res://example/main_menu/MainMenu.tscn",
        128u,
        "res://example/main_menu/MainMenu.tscn"
    )]
    public void ShortenPath_CorrectlyShortensPath(
        string path,
        uint length,
        string expected
    )
    {
        _ = Text.ShortenPath(path, (ushort)length).ConfirmEqual(expected);
    }
    #endregion ShortenPath

    #region ToStringInvariant
    [TestCase]
    public void ToStringInvariant_ShouldReturnEmptyString_WhenValueIsNull()
    {
        object? value = null;
        _ = value.ToStringInvariant().ConfirmEmpty();
    }

    [TestCase]
    public void ToStringInvariant_ShouldReturnInvariantCultureString_WhenValueIsNotNull()
    {
        _ = 12345.6789.ToStringInvariant().ConfirmEqual("12345.6789");
    }

    [TestCase]
    public void ToStringInvariant_ShouldHandleStringValuesCorrectly()
    {
        _ = "Hello, World!".ToStringInvariant().ConfirmEqual("Hello, World!");
    }

    [TestCase]
    public void ToStringInvariant_ShouldHandleDateTimeValuesCorrectly()
    {
        _ = new DateTime(2025, 2, 9, 15, 6, 45, DateTimeKind.Utc)
            .ToStringInvariant().ConfirmEqual("02/09/2025 15:06:45");
    }

    [TestCase]
    public void ToStringInvariant_ShouldHandleDecimalValuesCorrectly()
    {
        _ = 12345.6789m.ToStringInvariant().ConfirmEqual("12345.6789");
    }
    #endregion ToStringInvariant
}
