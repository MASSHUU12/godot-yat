using YAT.Helpers;

namespace GdUnit4
{
	using static Assertions;

	[TestSuite]
	public partial class TestText
	{
		[TestCase("Hello, world!", "Hello, world!")]
		[TestCase("Hello, [b]world[/b]!", "Hello, [lb]b]world[lb]/b]!")]
		[TestCase("[b]Hello[/b], world!", "[lb]b]Hello[lb]/b], world!")]
		[TestCase("[b]Hello, world![/b]", "[lb]b]Hello, world![lb]/b]")]
		[TestCase("[b]Hello, [b]world[/b]![/b]", "[lb]b]Hello, [lb]b]world[lb]/b]![lb]/b]")]
		[TestCase("[b]Hello, [b]world[/b]![/b] [b]Hello, [b]world[/b]![/b]", "[lb]b]Hello, [lb]b]world[lb]/b]![lb]/b] [lb]b]Hello, [lb]b]world[lb]/b]![lb]/b]")]
		public void TestEscapeBBCode(string text, string expected)
		{
			AssertThat(Text.EscapeBBCode(text)).IsEqual(expected);
		}

		[TestCase("Hello, world!", "[b]Hello, world![/b]")]
		[TestCase("Hello, [b]world[/b]!", "[b]Hello, [b]world[/b]![/b]")]
		[TestCase("[b]Hello[/b], world!", "[b][b]Hello[/b], world![/b]")]
		[TestCase("[b]Hello, world![/b]", "[b][b]Hello, world![/b][/b]")]
		[TestCase("[b]Hello, [b]world[/b]![/b]", "[b][b]Hello, [b]world[/b]![/b][/b]")]
		public void TestMakeBold(string text, string expected)
		{
			AssertThat(Text.MakeBold(text)).IsEqual(expected);
		}

		[TestCase("Hello, world!", new string[] { "Hello,", "world!" })]
		[TestCase("Hello, world! ", new string[] { "Hello,", "world!" })]
		[TestCase(" Hello, world!", new string[] { "Hello,", "world!" })]
		[TestCase(" Hello, world! ", new string[] { "Hello,", "world!" })]
		[TestCase("Hello,  world!", new string[] { "Hello,", "world!" })]
		[TestCase("  Hello,  world!    ", new string[] { "Hello,", "world!" })]
		public void TestSanitizeText(string text, string[] expected)
		{
			AssertArray(Text.SanitizeText(text)).IsEqual(expected);
		}

		[TestCase(new string[] { "Hello,", "world!" }, new string[] { "Hello,", "world!" })]
		[TestCase(new string[] { "echo", "\"Hello,", "world!\"" }, new string[] { "echo", "Hello, world!" })]
		[TestCase(new string[] { "echo", "'Hello,", "world!'" }, new string[] { "echo", "Hello, world!" })]
		[TestCase(new string[] { "qc", "add", "-name=\"John", "Doe\"" }, new string[] { "qc", "add", "-name=John Doe" })]
		[TestCase(new string[] { "qc", "add", "-name='John", "Doe'" }, new string[] { "qc", "add", "-name=John Doe" })]
		[TestCase(new string[] { "qc", "add", "-name='John", "\"Doe'" }, new string[] { "qc", "add", "-name=John \"Doe" })]
		public void TestConcatenateSentence(string[] strings, string[] expected)
		{
			AssertArray(Text.ConcatenateSentence(strings)).IsEqual(expected);
		}

		[TestCase("Hello, world!", true, new char[] { 'H', 'H', 'H' })]
		[TestCase("Hello, world!", true, new char[] { 'H', 'h' })]
		[TestCase("Hello, world!", true, new char[] { 'H', 'e' })]
		[TestCase("Hello, world!", true, new char[] { 'H', 'l' })]
		[TestCase("Hello, world!", true, new char[] { 'o', 'H' })]
		[TestCase("Hello, world!", false, new char[] { 'o', 'o' })]
		public void TestStartsWith(string text, bool expected, char[] chars)
		{
			AssertThat(Text.StartsWith(text, chars)).IsEqual(expected);
		}

		[TestCase("Hello, world!", true, new char[] { '!', '!', '!' })]
		[TestCase("Hello, world!", true, new char[] { '!', 'd' })]
		[TestCase("Hello, world!", true, new char[] { '!', 'l' })]
		[TestCase("Hello, world!", true, new char[] { '!', 'r' })]
		[TestCase("Hello, world!", true, new char[] { 'w', '!' })]
		[TestCase("Hello, world!", false, new char[] { 'o', 'o' })]
		public void TestEndsWith(string text, bool expected, char[] chars)
		{
			AssertThat(Text.EndsWith(text, chars)).IsEqual(expected);
		}

		[TestCase("Hello!", "", new string[] { "Hello!" })]
		[TestCase("Hello!", " ", new string[] { "Hello!" })]
		[TestCase("Hello!", "l", new string[] { "He", "o!" })]
		[TestCase("Hello!", "ll", new string[] { "He", "o!" })]
		[TestCase("Hello, World!", ", ", new string[] { "Hello", "World!" })]
		public void TestSplitClean(string text, string separator, string[] expected)
		{
			AssertArray(Text.SplitClean(text, separator)).IsEqual(expected);
		}

		[TestCase("res://example/main_menu/MainMenu.tscn", (ushort)128, "res://example/main_menu/MainMenu.tscn")]
		[TestCase("res://example/main_menu/MainMenu.tscn", (ushort)30, "...ple/main_menu/MainMenu.tscn")]
		[TestCase("res://example/main_menu/MainMenu.tscn", (ushort)15, "...ainMenu.tscn")]
		[TestCase("res://example/main_menu/MainMenu.tscn", (ushort)0, "...")]
		[TestCase("", (ushort)16, "...")]
		public void TestShortenPath(string path, ushort maxLength, string expected)
		{
			AssertThat(Text.ShortenPath(path, maxLength)).IsEqual(expected);
		}

		// Valid range and type, no array
		[TestCase("int(0:10)", "int", 0, 10, false, true)]
		[TestCase("int(5:10)", "int", 5, 10, false, true)]
		[TestCase("int(:10)", "int", 0, 10, false, true)]
		[TestCase("int(-12:8)", "int", 5, 10, false, true)]
		[TestCase("float(5:10)", "float", 5, 10, false, true)]
		[TestCase("float(5.5:10.5)", "float", 5.5, 10.5, false, true)]
		[TestCase("float(0.5:60)", "float", 0.5, 60, false, true)]
		[TestCase("float(0.5 : 60)", "float", 0.5, 60, false, true)]
		[TestCase("float(-0.5:60)", "float", 0.5, 60, false, true)]
		[TestCase("string(5:10)", "string", 5, 10, false, true)]
		// Invalid range and valid type, no array
		[TestCase("int(sda:da)", "int", 0, 0, false, false)]
		[TestCase("int(:dxdx)", "int", 0, 0, false, false)]
		[TestCase("float(:)", "float", 0, 0, false, false)]
		[TestCase("float( : )", "float", 0, 0, false, false)]
		[TestCase("string(5:)", "string", 0, 0, false, false)]
		[TestCase("string(x:10)", "string", 0, 0, false, false)]
		[TestCase("string(5:10:15)", "string", 0, 0, false, false)]
		[TestCase("string(5:x)", "string", 0, 0, false, false)]
		[TestCase("string(5::5)", "string", 0, 0, false, false)]
		[TestCase("int(:-5)", "int", 0, 0, false, false)]
		[TestCase("int(-12:-5)", "int", 0, 0, false, false)]
		[TestCase("int(10:5)", "int", 0, 0, false, false)]
		[TestCase("int()", "int", 0, 0, false, true)]
		// No range and valid type, no array
		[TestCase("int", "int", 0, 0, false, true)]
		[TestCase("choice", "choice", 0, 0, false, true)]
		[TestCase("float", "float", 0, 0, false, true)]
		[TestCase("string", "string", 0, 0, false, true)]
		[TestCase("x", "x", 0, 0, false, true)]
		// Valid range, type not allowed to have range, no array
		[TestCase("x(5:10)", "x", 0, 0, false, false)]
		[TestCase("choice(0:10)", "choice", 0, 0, false, false)]
		[TestCase("choice(5:10)", "choice", 0, 0, false, false)]
		[TestCase("y(:10)", "choice", 0, 0, false, false)]
		[TestCase("y(-12:8)", "choice", 0, 0, false, false)]
		// Invalid range and no type, no array
		[TestCase("( : )", "", 0, 0, false, false)]
		[TestCase("(:)", "", 0, 0, false, false)]
		[TestCase("()", "", 0, 0, false, false)]
		// No range and no type, no array
		[TestCase("", "", 0, 0, false, true)]
		// Valid type and array, no range
		[TestCase("int...", "int", 0, 0, true, true)]
		[TestCase("float...", "float", 0, 0, true, true)]
		[TestCase("string...", "string", 0, 0, true, true)]
		[TestCase("choice...", "choice", 0, 0, true, true)]
		// Valid range and type, array
		[TestCase("float(5:10)...", "float", 5, 10, true, true)]
		[TestCase("float(5.5:10.5)...", "float", 5.5, 10.5, true, true)]
		[TestCase("float(:60)...", "float", 0.5, 60, true, true)]
		// No range and no type, array
		[TestCase("...", "", 0, 0, true, false)]
		public static void TestTryParseCommandInputType(string type, string eType, float min, float max, bool isArray, bool successful)
		{
			AssertThat(Text.TryParseCommandInputType(type, out var result)).IsEqual(successful);

			if (!successful) return;

			AssertThat(result.Type).IsEqual(eType);
			AssertThat(result.Min).IsEqual(min);
			AssertThat(result.Max).IsEqual(max);
			AssertThat(result.IsArray).IsEqual(isArray);
		}
	}
}
