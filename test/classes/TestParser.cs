using System;
using Confirma;
using Confirma.Attributes;
using Confirma.Classes;
using YAT.Classes;

namespace YAT.Test;

[TestClass]
public static class TestParser
{
	[TestCase("", new string[] { })]
	[TestCase("test command", new string[] { "test", "command" })]
	[TestCase("test command with spaces", new string[] { "test", "command", "with", "spaces" })]
	[TestCase("cn ?Cube", new string[] { "cn", "?Cube" })]
	[TestCase("ls -l", new string[] { "ls", "-l" })]
	[TestCase("ping test.com -bytes=48", new string[] { "ping", "test.com", "-bytes=48" })]
	[TestCase("echo \"Hello, World\"", new string[] { "echo", "Hello, World" })]
	public static void ParseCommand(string command, string[] expected)
	{
		Parser.ParseCommand(command).ConfirmEqual(expected);
	}

	[TestCase("", "", "")]
	[TestCase("testMethod()", "testMethod", "")]
	[TestCase("test_method", "test_method", "")]
	[TestCase("testMethod(arg1)", "testMethod", "arg1")]
	[TestCase("testMethod(arg1, arg2)", "testMethod", "arg1, arg2")]
	[TestCase("testMethod(arg1, arg2, arg3)", "testMethod", "arg1, arg2, arg3")]
	public static void ParseMethod(string method, string expectedName, string expectedArgs)
	{
		var result = Parser.ParseMethod(method);
		result.Item1.ConfirmEqual(expectedName);
		result.Item2.ConfirmEqual(expectedArgs.Split(", ", StringSplitOptions.RemoveEmptyEntries));
	}

	// Valid range and type, no array
	[TestCase("int(0:10)", "int", 0, 10, false, true)]
	[TestCase("int(5:10)", "int", 5, 10, false, true)]
	[TestCase("int(-12:8)", "int", -12, 8, false, true)]
	[TestCase("float(5:10)", "float", 5, 10, false, true)]
	[TestCase("int(-12:-5)", "int", -12, -5, false, true)]
	[TestCase("string(5:10)", "string", 5, 10, false, true)]
	[TestCase("float(0.5:60)", "float", 0.5f, 60, false, true)]
	[TestCase("float(0.5 : 60)", "float", 0.5f, 60, false, true)]
	[TestCase("float(-0.5:60)", "float", -0.5f, 60, false, true)]
	[TestCase("int(:10)", "int", float.MinValue, 10, false, true)]
	[TestCase("int(:-5)", "int", float.MinValue, -5, false, true)]
	[TestCase("float(5.5:10.5)", "float", 5.5f, 10.5f, false, true)]
	[TestCase("string(5:)", "string", 5, float.MaxValue, false, true)]
	[TestCase("int()", "int", float.MinValue, float.MaxValue, false, true)]
	// Invalid range and valid type, no array
	[TestCase("int(10:5)", "int", 0, 0, false, false)]
	[TestCase("float(:)", "float", 0, 0, false, false)]
	[TestCase("int(:dxdx)", "int", 0, 0, false, false)]
	[TestCase("int(sda:da)", "int", 0, 0, false, false)]
	[TestCase("float( : )", "float", 0, 0, false, false)]
	[TestCase("string(5:x)", "string", 0, 0, false, false)]
	[TestCase("string(x:10)", "string", 0, 0, false, false)]
	[TestCase("string(5::5)", "string", 0, 0, false, false)]
	[TestCase("string(5:10:15)", "string", 0, 0, false, false)]
	// No range and valid type, no array
	[TestCase("x", "x", float.MinValue, float.MaxValue, false, true)]
	[TestCase("int", "int", float.MinValue, float.MaxValue, false, true)]
	[TestCase("float", "float", float.MinValue, float.MaxValue, false, true)]
	[TestCase("choice", "choice", float.MinValue, float.MaxValue, false, true)]
	[TestCase("string", "string", float.MinValue, float.MaxValue, false, true)]
	// Valid range, type not allowed to have range, no array
	[TestCase("x(5:10)", "x", 0, 0, false, false)]
	[TestCase("y(:10)", "choice", 0, 0, false, false)]
	[TestCase("y(-12:8)", "choice", 0, 0, false, false)]
	[TestCase("choice(5:10)", "choice", 0, 0, false, false)]
	[TestCase("choice(0:10)", "choice", 0, 0, false, false)]
	// Invalid range and no type, no array
	[TestCase("()", "", 0, 0, false, false)]
	[TestCase("(:)", "", 0, 0, false, false)]
	[TestCase("( : )", "", 0, 0, false, false)]
	// No range and no type, no array
	[TestCase("", "", 0, 0, false, false)]
	// Valid type and array, no range
	[TestCase("int...", "int", float.MinValue, float.MaxValue, true, true)]
	[TestCase("float...", "float", float.MinValue, float.MaxValue, true, true)]
	[TestCase("string...", "string", float.MinValue, float.MaxValue, true, true)]
	[TestCase("choice...", "choice", float.MinValue, float.MaxValue, true, true)]
	// Valid range and type, array
	[TestCase("float(5:10)...", "float", 5, 10, true, true)]
	[TestCase("float(5.5:10.5)...", "float", 5.5f, 10.5f, true, true)]
	[TestCase("float(:60)...", "float", float.MinValue, 60, true, true)]
	// No range and no type, array
	[TestCase("...", "", float.MinValue, float.MaxValue, true, false)]
	public static void TryParseCommandInputType(
		string type,
		string eType,
		float min,
		float max,
		bool isArray,
		bool isSuccess
	)
	{
		Parser.TryParseCommandInputType(type, out var result).ConfirmEqual(isSuccess);

		if (!isSuccess) return;

		result.Type.ToString().ConfirmEqual(eType);
		result.Min.ConfirmEqual(min);
		result.Max.ConfirmEqual(max);
		result.IsArray.ConfirmEqual(isArray);
	}
}
