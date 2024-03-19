using System;
using Chickensoft.GoDotTest;
using Godot;
using Shouldly;
using YAT.Classes;

namespace Test;

public class TestParser : TestClass
{
	public TestParser(Node testScene) : base(testScene) { }

	[Test]
	public static void TestParseCommand()
	{
		ParseCommand("", Array.Empty<string>());
		ParseCommand("test command", new string[] { "test", "command" });
		ParseCommand("test command with spaces", new string[] { "test", "command", "with", "spaces" });
		ParseCommand("cn ?Cube", new string[] { "cn", "?Cube" });
		ParseCommand("ls -l", new string[] { "ls", "-l" });
		ParseCommand("ping test.com -bytes=48", new string[] { "ping", "test.com", "-bytes=48" });
		ParseCommand("echo \"Hello, World\"", new string[] { "echo", "Hello, World" });
	}

	private static void ParseCommand(string command, string[] expected)
	{
		Parser.ParseCommand(command).ShouldBe(expected);
	}

	[Test]
	public static void TestParseMethod()
	{
		ParseMethod("", "", string.Empty);
		ParseMethod("testMethod()", "testMethod", string.Empty);
		ParseMethod("test_method", "test_method", string.Empty);
		ParseMethod("testMethod(arg1)", "testMethod", "arg1");
		ParseMethod("testMethod(arg1, arg2)", "testMethod", "arg1, arg2");
		ParseMethod("testMethod(arg1, arg2, arg3)", "testMethod", "arg1, arg2, arg3");
	}

	private static void ParseMethod(string method, string expectedName, string expectedArgs)
	{
		var result = Parser.ParseMethod(method);
		result.Item1.ShouldBe(expectedName);
		result.Item2.ShouldBe(expectedArgs.Split(", ", StringSplitOptions.RemoveEmptyEntries));
	}

	[Test]
	public static void TestTryParseCommandInputType()
	{
		// Valid range and type, no array
		TryParseCommandInputType("int(0:10)", "int", 0, 10, false, true);
		TryParseCommandInputType("int(5:10)", "int", 5, 10, false, true);
		TryParseCommandInputType("int(:10)", "int", float.MinValue, 10, false, true);
		TryParseCommandInputType("int(-12:8)", "int", -12, 8, false, true);
		TryParseCommandInputType("float(5:10)", "float", 5, 10, false, true);
		TryParseCommandInputType("float(5.5:10.5)", "float", 5.5f, 10.5f, false, true);
		TryParseCommandInputType("float(0.5:60)", "float", 0.5f, 60, false, true);
		TryParseCommandInputType("float(0.5 : 60)", "float", 0.5f, 60, false, true);
		TryParseCommandInputType("float(-0.5:60)", "float", -0.5f, 60, false, true);
		TryParseCommandInputType("string(5:10)", "string", 5, 10, false, true);
		TryParseCommandInputType("string(5:)", "string", 5, float.MaxValue, false, true);
		TryParseCommandInputType("int(:-5)", "int", float.MinValue, -5, false, true);
		TryParseCommandInputType("int(-12:-5)", "int", -12, -5, false, true);
		// TryParseCommandInputType("float(:)", "float", float.MinValue, float.MaxValue, false, true);
		TryParseCommandInputType("int()", "int", float.MinValue, float.MaxValue, false, true);
		// Invalid range and valid type, no array
		TryParseCommandInputType("int(sda:da)", "int", 0, 0, false, false);
		TryParseCommandInputType("int(:dxdx)", "int", 0, 0, false, false);
		TryParseCommandInputType("float( : )", "float", 0, 0, false, false);
		TryParseCommandInputType("string(x:10)", "string", 0, 0, false, false);
		TryParseCommandInputType("string(5:10:15)", "string", 0, 0, false, false);
		TryParseCommandInputType("string(5:x)", "string", 0, 0, false, false);
		TryParseCommandInputType("string(5::5)", "string", 0, 0, false, false);
		TryParseCommandInputType("int(10:5)", "int", 0, 0, false, false);
		// No range and valid type, no array
		TryParseCommandInputType("int", "int", float.MinValue, float.MaxValue, false, true);
		TryParseCommandInputType("choice", "choice", float.MinValue, float.MaxValue, false, true);
		TryParseCommandInputType("float", "float", float.MinValue, float.MaxValue, false, true);
		TryParseCommandInputType("string", "string", float.MinValue, float.MaxValue, false, true);
		TryParseCommandInputType("x", "x", float.MinValue, float.MaxValue, false, true);
		// Valid range, type not allowed to have range, no array
		TryParseCommandInputType("x(5:10)", "x", 0, 0, false, false);
		TryParseCommandInputType("choice(0:10)", "choice", 0, 0, false, false);
		TryParseCommandInputType("choice(5:10)", "choice", 0, 0, false, false);
		TryParseCommandInputType("y(:10)", "choice", 0, 0, false, false);
		TryParseCommandInputType("y(-12:8)", "choice", 0, 0, false, false);
		// Invalid range and no type, no array
		// TryParseCommandInputType("( : )", "", 0, 0, false, false);
		// TryParseCommandInputType("(:)", "", 0, 0, false, false);
		// TryParseCommandInputType("()", "", 0, 0, false, false);
		// No range and no type, no array
		TryParseCommandInputType("", "", 0, 0, false, false);
		// Valid type and array, no range
		TryParseCommandInputType("int...", "int", float.MinValue, float.MaxValue, true, true);
		TryParseCommandInputType("float...", "float", float.MinValue, float.MaxValue, true, true);
		TryParseCommandInputType("string...", "string", float.MinValue, float.MaxValue, true, true);
		TryParseCommandInputType("choice...", "choice", float.MinValue, float.MaxValue, true, true);
		// Valid range and type, array
		TryParseCommandInputType("float(5:10)...", "float", 5, 10, true, true);
		TryParseCommandInputType("float(5.5:10.5)...", "float", 5.5f, 10.5f, true, true);
		TryParseCommandInputType("float(:60)...", "float", float.MinValue, 60, true, true);
		// No range and no type, array
		TryParseCommandInputType("...", "", float.MinValue, float.MaxValue, true, false);
	}

	private static void TryParseCommandInputType(string type, string eType, float min, float max, bool isArray, bool isSuccess)
	{
		Parser.TryParseCommandInputType(type, out var result).ShouldBe(isSuccess);

		if (!isSuccess) return;

		result.Type.ToString().ShouldBe(eType);
		result.Min.ShouldBe(min);
		result.Max.ShouldBe(max);
		result.IsArray.ShouldBe(isArray);
	}
}
