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
	}
}
