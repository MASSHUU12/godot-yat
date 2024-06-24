using System;
using Confirma.Attributes;
using Confirma.Classes;
using Confirma.Extensions;
using YAT.Classes;
using YAT.Enums;
using static YAT.Enums.ECommandInputType;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class ParserTest
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
	[TestCase("int(0:10)", Int, 0, 10, false, true)]
	[TestCase("int(5:10)", Int, 5, 10, false, true)]
	[TestCase("int(-12:8)", Int, -12, 8, false, true)]
	[TestCase("float(5:10)", Float, 5, 10, false, true)]
	[TestCase("int(-12:-5)", Int, -12, -5, false, true)]
	[TestCase("string(5:10)", ECommandInputType.String, 5, 10, false, true)]
	[TestCase("float(0.5:60)", Float, 0.5f, 60, false, true)]
	[TestCase("float(0.5 : 60)", Float, 0.5f, 60, false, true)]
	[TestCase("float(-0.5:60)", Float, -0.5f, 60, false, true)]
	[TestCase("int(:10)", Int, float.MinValue, 10, false, true)]
	[TestCase("int(:-5)", Int, float.MinValue, -5, false, true)]
	[TestCase("float(5.5:10.5)", Float, 5.5f, 10.5f, false, true)]
	[TestCase("string(5:)", ECommandInputType.String, 5, float.MaxValue, false, true)]
	[TestCase("int()", Int, float.MinValue, float.MaxValue, false, true)]
	// Invalid range and valid type, no array
	[TestCase("int(10:5)", Int, 0, 0, false, false)]
	[TestCase("float(:)", Float, 0, 0, false, false)]
	[TestCase("int(:dxdx)", Int, 0, 0, false, false)]
	[TestCase("int(sda:da)", Int, 0, 0, false, false)]
	[TestCase("float( : )", Float, 0, 0, false, false)]
	[TestCase("string(5:x)", ECommandInputType.String, 0, 0, false, false)]
	[TestCase("string(x:10)", ECommandInputType.String, 0, 0, false, false)]
	[TestCase("string(5::5)", ECommandInputType.String, 0, 0, false, false)]
	[TestCase("string(5:10:15)", ECommandInputType.String, 0, 0, false, false)]
	// No range and valid type, no array
	[TestCase("x", Constant, 0, 0, false, true)]
	[TestCase("int", Int, 0, 0, false, true)]
	[TestCase("float", Float, 0, 0, false, true)]
	[TestCase("choice", Constant, 0, 0, false, true)]
	[TestCase("string", ECommandInputType.String, 0, 0, false, true)]
	// Valid range, type not allowed to have range, no array
	[TestCase("x(5:10)", ECommandInputType.Void, 0, 0, false, false)]
	[TestCase("y(:10)", ECommandInputType.Void, 0, 0, false, false)]
	[TestCase("y(-12:8)", ECommandInputType.Void, 0, 0, false, false)]
	[TestCase("choice(5:10)", ECommandInputType.Void, 0, 0, false, false)]
	[TestCase("choice(0:10)", ECommandInputType.Void, 0, 0, false, false)]
	// Invalid range and no type, no array
	[TestCase("()", ECommandInputType.Void, 0, 0, false, false)]
	[TestCase("(:)", ECommandInputType.Void, 0, 0, false, false)]
	[TestCase("( : )", ECommandInputType.Void, 0, 0, false, false)]
	// No range and no type, no array
	[TestCase("", ECommandInputType.Void, 0, 0, false, false)]
	// Valid type and array, no range
	[TestCase("int...", Int, float.MinValue, float.MaxValue, true, true)]
	[TestCase("float...", Float, float.MinValue, float.MaxValue, true, true)]
	[TestCase("string...", ECommandInputType.String, float.MinValue, float.MaxValue, true, true)]
	[TestCase("choice...", Constant, 0, 0, true, true)]
	// Valid range and type, array
	[TestCase("float(5:10)...", Float, 5, 10, true, true)]
	[TestCase("float(5.5:10.5)...", Float, 5.5f, 10.5f, true, true)]
	[TestCase("float(:60)...", Float, float.MinValue, 60, true, true)]
	// No range and no type, array
	[TestCase("...", ECommandInputType.Void, 0, 0, true, false)]
	public static void TryParseCommandInputType(
		string type,
		ECommandInputType eType,
		float min,
		float max,
		bool isArray,
		bool isSuccess
	)
	{
		Parser.TryParseCommandInputType(type, out var result).ConfirmEqual(isSuccess);

		if (!isSuccess) return;

		result.Type.ConfirmEqual(eType);
		result.IsArray.ConfirmEqual(isArray);

		if (result is CommandTypeRanged res)
		{
			res.Min.ConfirmEqual(min);
			res.Max.ConfirmEqual(max);
		}
	}

	// Valid range and type, no array
	[TestCase(Int, "0:10", Int, 0, 10, false, true)]
	[TestCase(Int, "5:10", Int, 5, 10, false, true)]
	[TestCase(Int, "-12:8", Int, -12, 8, false, true)]
	[TestCase(Float, "5:10", Float, 5, 10, false, true)]
	[TestCase(Int, "-12:-5", Int, -12, -5, false, true)]
	[TestCase(ECommandInputType.String, "5:10", ECommandInputType.String, 5, 10, false, true)]
	[TestCase(Float, "0.5:60", Float, 0.5f, 60, false, true)]
	[TestCase(Float, "0.5 : 60", Float, 0.5f, 60, false, true)]
	[TestCase(Float, "-0.5:60", Float, -0.5f, 60, false, true)]
	[TestCase(Int, ":10", Int, float.MinValue, 10, false, true)]
	[TestCase(Int, ":-5", Int, float.MinValue, -5, false, true)]
	[TestCase(Float, "5.5:10.5", Float, 5.5f, 10.5f, false, true)]
	[TestCase(ECommandInputType.String, "5:", ECommandInputType.String, 5, float.MaxValue, false, true)]
	[TestCase(Int, "", Int, float.MinValue, float.MaxValue, false, true)]
	// Invalid range and valid type, no array
	[TestCase(Float, ":", Float, 0, 0, false, false)]
	[TestCase(Int, ":dxdx", Int, 0, 0, false, false)]
	[TestCase(Int, "sda:da", Int, 0, 0, false, false)]
	[TestCase(Float, " : ", Float, 0, 0, false, false)]
	[TestCase(ECommandInputType.String, "5:x", ECommandInputType.String, 0, 0, false, false)]
	[TestCase(ECommandInputType.String, "x:10", ECommandInputType.String, 0, 0, false, false)]
	[TestCase(ECommandInputType.String, "5::5", ECommandInputType.String, 0, 0, false, false)]
	[TestCase(ECommandInputType.String, "5:10:15", ECommandInputType.String, 0, 0, false, false)]
	// No range and valid type, no array
	[TestCase(Int, "", Int, float.MinValue, float.MaxValue, false, true)]
	[TestCase(Float, "", Float, float.MinValue, float.MaxValue, false, true)]
	[TestCase(ECommandInputType.String, "", ECommandInputType.String, float.MinValue, float.MaxValue, false, true)]
	// Valid type and array, no range
	[TestCase(Int, "", Int, float.MinValue, float.MaxValue, true, true)]
	[TestCase(Float, "", Float, float.MinValue, float.MaxValue, true, true)]
	[TestCase(ECommandInputType.String, "", ECommandInputType.String, float.MinValue, float.MaxValue, true, true)]
	// Valid range and type, array
	[TestCase(Float, "5:10", Float, 5, 10, true, true)]
	[TestCase(Float, "5.5:10.5", Float, 5.5f, 10.5f, true, true)]
	[TestCase(Float, ":60", Float, float.MinValue, 60, true, true)]
	public static void TryParseTypeWithRange(
		ECommandInputType type,
		string range,
		ECommandInputType eType,
		float eMin,
		float eMax,
		bool isArray,
		bool isSuccess
	)
	{
		Parser.TryParseTypeWithRange(type, range, isArray, out var result).ConfirmEqual(isSuccess);

		if (!isSuccess) return;

		result.Type.ConfirmEqual(eType);
		result.Min.ConfirmEqual(eMin);
		result.Max.ConfirmEqual(eMax);
		result.IsArray.ConfirmEqual(isArray);
	}

	[TestCase("Void", ECommandInputType.Void, true)]
	[TestCase("void", ECommandInputType.Void, true)]
	[TestCase("int", Int, true)]
	[TestCase("enum?", ECommandInputType.Void, false)]
	public static void TryParseStringTypeToEnum(string type, ECommandInputType expected, bool shouldBeSuccess)
	{
		var isSuccess = Parser.TryParseStringTypeToEnum(type, out var result);

		isSuccess.ConfirmEqual(shouldBeSuccess);
		result.ConfirmEqual(expected);
	}
}
