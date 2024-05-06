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
	public static void TestEscapeBBCode(string text, string expected)
	{
		Text.EscapeBBCode(text).ConfirmEqual(expected);
	}

	[TestCase("Hello, world!", "[b]Hello, world![/b]")]
	[TestCase("Hello, [b]world[/b]!", "[b]Hello, [b]world[/b]![/b]")]
	[TestCase("[b]Hello[/b], world!", "[b][b]Hello[/b], world![/b]")]
	[TestCase("[b]Hello, world![/b]", "[b][b]Hello, world![/b][/b]")]
	[TestCase("[b]Hello, [b]world[/b]![/b]", "[b][b]Hello, [b]world[/b]![/b][/b]")]
	public static void TestMakeBold(string text, string expected)
	{
		Text.MakeBold(text).ConfirmEqual(expected);
	}

	[TestCase("Hello, world!", new string[] { "Hello,", "world!" })]
	[TestCase("Hello, world! ", new string[] { "Hello,", "world!" })]
	[TestCase(" Hello, world!", new string[] { "Hello,", "world!" })]
	[TestCase(" Hello, world! ", new string[] { "Hello,", "world!" })]
	[TestCase("Hello,  world!", new string[] { "Hello,", "world!" })]
	[TestCase("  Hello,  world!    ", new string[] { "Hello,", "world!" })]
	public static void TestSanitizeTest(string text, string[] expected)
	{
		Text.SanitizeText(text).ConfirmEqual(expected);
	}

	[TestCase(new string[] { "Hello,", "world!" }, new string[] { "Hello,", "world!" })]
	[TestCase(new string[] { "echo", "\"Hello,", "world!\"" }, new string[] { "echo", "Hello, world!" })]
	[TestCase(new string[] { "echo", "'Hello,", "world!'" }, new string[] { "echo", "Hello, world!" })]
	[TestCase(new string[] { "qc", "add", "-name=\"John", "Doe\"" }, new string[] { "qc", "add", "-name=John Doe" })]
	[TestCase(new string[] { "qc", "add", "-name='John", "Doe'" }, new string[] { "qc", "add", "-name=John Doe" })]
	[TestCase(new string[] { "qc", "add", "-name='John", "\"Doe'" }, new string[] { "qc", "add", "-name=John \"Doe" })]
	public static void TestConcatenateSentence(string[] strings, string[] expected)
	{
		Text.ConcatenateSentence(strings).ConfirmEqual(expected);
	}

	[TestCase("Hello, world!", true, new char[] { 'H', 'H', 'H' })]
	[TestCase("Hello, world!", true, new char[] { 'H', 'h' })]
	[TestCase("Hello, world!", true, new char[] { 'H', 'e' })]
	[TestCase("Hello, world!", true, new char[] { 'H', 'l' })]
	[TestCase("Hello, world!", true, new char[] { 'o', 'H' })]
	[TestCase("Hello, world!", false, new char[] { 'o', 'o' })]
	public static void TestStartsWith(string text, bool expected, char[] chars)
	{
		Text.StartsWith(text, chars).ConfirmEqual(expected);
	}

	[TestCase("Hello, world!", true, new char[] { '!', '!', '!' })]
	[TestCase("Hello, world!", true, new char[] { '!', 'd' })]
	[TestCase("Hello, world!", true, new char[] { '!', 'l' })]
	[TestCase("Hello, world!", true, new char[] { '!', 'r' })]
	[TestCase("Hello, world!", true, new char[] { 'w', '!' })]
	public static void TestEndsWith(string text, bool expected, char[] chars)
	{
		Text.EndsWith(text, chars).ConfirmEqual(expected);
	}

	[TestCase("Hello!", "", new string[] { "Hello!" })]
	[TestCase("Hello!", " ", new string[] { "Hello!" })]
	[TestCase("Hello!", "l", new string[] { "He", "o!" })]
	[TestCase("Hello!", "ll", new string[] { "He", "o!" })]
	[TestCase("Hello, World!", ", ", new string[] { "Hello", "World!" })]
	public static void TestSplitClean(string text, string separator, string[] expected)
	{
		Text.SplitClean(text, separator).ConfirmEqual(expected);
	}

	[TestCase("", 16u, "...")]
	[TestCase("res://example/main_menu/MainMenu.tscn", 0u, "...")]
	[TestCase("res://example/main_menu/MainMenu.tscn", 15u, "res:/.../.../..")]
	[TestCase("res://example/main_menu/MainMenu.tscn", 30u, "res://.../.../MainMenu.tscn")]
	[TestCase("res://example/main_menu/MainMenu.tscn", 128u, "res://example/main_menu/MainMenu.tscn")]
	public static void TestShortenPath(string path, uint length, string expected)
	{
		Text.ShortenPath(path, (ushort)length).ConfirmEqual(expected);
	}
}
