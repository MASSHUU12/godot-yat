using Chickensoft.GoDotTest;
using Godot;
using YAT.Helpers;
using Shouldly;

namespace Test;

public class TestText : TestClass
{
	public TestText(Node testScene) : base(testScene) { }

	[Test]
	public static void TestEscapeBBCode()
	{
		EscapeBBCode("Hello, world!", "Hello, world!");
		EscapeBBCode("Hello, [b]world[/b]!", "Hello, [lb]b]world[lb]/b]!");
		EscapeBBCode("[b]Hello[/b], world!", "[lb]b]Hello[lb]/b], world!");
		EscapeBBCode("[b]Hello, world![/b]", "[lb]b]Hello, world![lb]/b]");
		EscapeBBCode("[b]Hello, [b]world[/b]![/b]", "[lb]b]Hello, [lb]b]world[lb]/b]![lb]/b]");
		EscapeBBCode("[b]Hello, [b]world[/b]![/b] [b]Hello, [b]world[/b]![/b]", "[lb]b]Hello, [lb]b]world[lb]/b]![lb]/b] [lb]b]Hello, [lb]b]world[lb]/b]![lb]/b]");
	}

	private static void EscapeBBCode(string text, string expected)
	{
		Text.EscapeBBCode(text).ShouldBe(expected);
	}

	[Test]
	public static void TestMakeBold()
	{
		MakeBold("Hello, world!", "[b]Hello, world![/b]");
		MakeBold("Hello, [b]world[/b]!", "[b]Hello, [b]world[/b]![/b]");
		MakeBold("[b]Hello[/b], world!", "[b][b]Hello[/b], world![/b]");
		MakeBold("[b]Hello, world![/b]", "[b][b]Hello, world![/b][/b]");
		MakeBold("[b]Hello, [b]world[/b]![/b]", "[b][b]Hello, [b]world[/b]![/b][/b]");
	}

	private static void MakeBold(string text, string expected)
	{
		Text.MakeBold(text).ShouldBe(expected);
	}

	[Test]
	public static void TestSanitizeText()
	{
		SanitizeText("Hello, world!", new string[] { "Hello,", "world!" });
		SanitizeText("Hello, world! ", new string[] { "Hello,", "world!" });
		SanitizeText(" Hello, world!", new string[] { "Hello,", "world!" });
		SanitizeText(" Hello, world! ", new string[] { "Hello,", "world!" });
		SanitizeText("Hello,  world!", new string[] { "Hello,", "world!" });
		SanitizeText("  Hello,  world!    ", new string[] { "Hello,", "world!" });
	}

	private static void SanitizeText(string text, string[] expected)
	{
		Text.SanitizeText(text).ShouldBe(expected);
	}

	[Test]
	public static void TestConcatenateSentence()
	{
		ConcatenateSentence(new string[] { "Hello,", "world!" }, new string[] { "Hello,", "world!" });
		ConcatenateSentence(new string[] { "echo", "\"Hello,", "world!\"" }, new string[] { "echo", "Hello, world!" });
		ConcatenateSentence(new string[] { "echo", "'Hello,", "world!'" }, new string[] { "echo", "Hello, world!" });
		ConcatenateSentence(new string[] { "qc", "add", "-name=\"John", "Doe\"" }, new string[] { "qc", "add", "-name=John Doe" });
		ConcatenateSentence(new string[] { "qc", "add", "-name='John", "Doe'" }, new string[] { "qc", "add", "-name=John Doe" });
		ConcatenateSentence(new string[] { "qc", "add", "-name='John", "\"Doe'" }, new string[] { "qc", "add", "-name=John \"Doe" });
	}

	private static void ConcatenateSentence(string[] strings, string[] expected)
	{
		Text.ConcatenateSentence(strings).ShouldBe(expected);
	}

	[Test]
	public static void TestStartsWith()
	{
		StartsWith("Hello, world!", true, new char[] { 'H', 'H', 'H' });
		StartsWith("Hello, world!", true, new char[] { 'H', 'h' });
		StartsWith("Hello, world!", true, new char[] { 'H', 'e' });
		StartsWith("Hello, world!", true, new char[] { 'H', 'l' });
		StartsWith("Hello, world!", true, new char[] { 'o', 'H' });
		StartsWith("Hello, world!", false, new char[] { 'o', 'o' });
	}

	private static void StartsWith(string text, bool expected, char[] chars)
	{
		Text.StartsWith(text, chars).ShouldBe(expected);
	}

	[Test]
	public static void TestEndsWith()
	{
		EndsWith("Hello, world!", true, new char[] { '!', '!', '!' });
		EndsWith("Hello, world!", true, new char[] { '!', 'd' });
		EndsWith("Hello, world!", true, new char[] { '!', 'l' });
		EndsWith("Hello, world!", true, new char[] { '!', 'r' });
		EndsWith("Hello, world!", true, new char[] { 'w', '!' });
	}

	private static void EndsWith(string text, bool expected, char[] chars)
	{
		Text.EndsWith(text, chars).ShouldBe(expected);
	}

	[Test]
	public static void TestSplitClean()
	{
		SplitClean("Hello!", "", new string[] { "Hello!" });
		SplitClean("Hello!", " ", new string[] { "Hello!" });
		SplitClean("Hello!", "l", new string[] { "He", "o!" });
		SplitClean("Hello!", "ll", new string[] { "He", "o!" });
		SplitClean("Hello, World!", ", ", new string[] { "Hello", "World!" });
	}

	private static void SplitClean(string text, string separator, string[] expected)
	{
		Text.SplitClean(text, separator).ShouldBe(expected);
	}

	[Test]
	public static void TestShortenPath()
	{
		ShortenPath("res://example/main_menu/MainMenu.tscn", 128, "res://example/main_menu/MainMenu.tscn");
		ShortenPath("res://example/main_menu/MainMenu.tscn", 30, "res://.../.../MainMenu.tscn");
		ShortenPath("res://example/main_menu/MainMenu.tscn", 15, "res:/.../.../..");
		ShortenPath("res://example/main_menu/MainMenu.tscn", 0, "...");
		ShortenPath("", 16, "...");
	}

	private static void ShortenPath(string path, ushort maxLength, string expected)
	{
		Text.ShortenPath(path, maxLength).ShouldBe(expected);
	}
}
