namespace GdUnit4
{
	using YAT.Classes;
	using static Assertions;

	[TestSuite]
	public partial class TestParser
	{
		[TestCase("", new string[] { "" })]
		[TestCase("test command", new string[] { "test", "command" })]
		[TestCase("test command with spaces", new string[] { "test", "command", "with", "spaces" })]
		[TestCase("cn ?Cube", new string[] { "cn", "?Cube" })]
		[TestCase("ls -l", new string[] { "ls", "-l" })]
		[TestCase("ping test.com -bytes=48", new string[] { "ping", "test.com", "-bytes=48" })]
		[TestCase("echo \"Hello, World\"", new string[] { "echo", "Hello, World" })]
		public void TestParseCommand(string command, string[] expected)
		{
			var result = Parser.ParseCommand(command);
			AssertArray(expected).IsEqual(result);
		}

		[TestCase("", "", "")]
		[TestCase("testMethod()", "testMethod", "")]
		[TestCase("test_method", "test_method", "")]
		[TestCase("testMethod(arg1)", "testMethod", "arg1")]
		[TestCase("testMethod(arg1, arg2)", "testMethod", "arg1, arg2")] // !
		[TestCase("testMethod(arg1, arg2, arg3)", "testMethod", "arg1, arg2, arg3")]
		public void TestParseMethod(string method, string expectedName, string expectedArgs)
		{
			var result = Parser.ParseMethod(method);
			AssertString(expectedName).IsEqual(result.Item1);
			AssertArray(expectedArgs.Split(", ")).IsEqual(result.Item2);
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
			AssertThat(Parser.TryParseCommandInputType(type, out var result)).IsEqual(successful);

			if (!successful) return;

			AssertThat(result.Type).IsEqual(eType);
			AssertThat(result.Min).IsEqual(min);
			AssertThat(result.Max).IsEqual(max);
			AssertThat(result.IsArray).IsEqual(isArray);
		}
	}
}
