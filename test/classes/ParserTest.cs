using System;
using Confirma.Attributes;
using Confirma.Classes;
using Confirma.Extensions;
using YAT.Classes;
using YAT.Enums;
using YAT.Helpers;
using static YAT.Enums.ECommandInputType;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class ParserTest
{
    private static readonly Random _rg = new();

    [TestCase("", new string[] { })]
    [TestCase("test command", new string[] { "test", "command" })]
    [TestCase("test command with spaces", new string[] { "test", "command", "with", "spaces" })]
    [TestCase("cn ?Cube", new string[] { "cn", "?Cube" })]
    [TestCase("ls -l", new string[] { "ls", "-l" })]
    [TestCase("ping test.com -bytes=48", new string[] { "ping", "test.com", "-bytes=48" })]
    [TestCase("echo \"Hello, World\"", new string[] { "echo", "Hello, World" })]
    public static void ParseCommand(string command, string[] expected)
    {
        _ = Parser.ParseCommand(command).ConfirmEqual(expected);
    }

    [TestCase("", "", "")]
    [TestCase("testMethod()", "testMethod", "")]
    [TestCase("test_method", "test_method", "")]
    [TestCase("testMethod(arg1)", "testMethod", "arg1")]
    [TestCase("testMethod(arg1, arg2)", "testMethod", "arg1, arg2")]
    [TestCase("testMethod(arg1, arg2, arg3)", "testMethod", "arg1, arg2, arg3")]
    public static void ParseMethod(string method, string expectedName, string expectedArgs)
    {
        (string, string[]) result = Parser.ParseMethod(method);
        _ = result.Item1.ConfirmEqual(expectedName);
        _ = result.Item2.ConfirmEqual(expectedArgs.Split(", ", StringSplitOptions.RemoveEmptyEntries));
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
    [TestCase("x", Constant, float.MinValue, float.MaxValue, false, true)]
    [TestCase("int", Int, float.MinValue, float.MaxValue, false, true)]
    [TestCase("float", Float, float.MinValue, float.MaxValue, false, true)]
    [TestCase("choice", Constant, float.MinValue, float.MaxValue, false, true)]
    [TestCase("string", ECommandInputType.String, float.MinValue, float.MaxValue, false, true)]
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
        _ = Parser.TryParseCommandInputType(type, out var result).ConfirmEqual(isSuccess);

        if (!isSuccess)
        {
            return;
        }

        _ = result.Type.ConfirmEqual(eType);
        _ = result.IsArray.ConfirmEqual(isArray);

        if (!((int)result.Type).IsWithinRange<int>(1, 3))
        {
            return;
        }

        CommandTypeRanged r = result.ConfirmType<CommandTypeRanged>();
        _ = r.Min.ConfirmEqual(min);
        _ = r.Max.ConfirmEqual(max);
    }

    #region TryParseCommandInputType_Enum
    [TestCase(
        "enum(A:1,B:2,C:3)",
        new string[] { "A", "B", "C" },
        new int[] { 1, 2, 3 }
    )]
    [TestCase(
        "enum(d:1,E:2,f:3)",
        new string[] { "d", "E", "f" },
        new int[] { 1, 2, 3 }
    )]
    [TestCase(
        "enum(G: -5, H:13, I : 3)",
        new string[] { "G", "H", "I" },
        new int[] { -5, 13, 3 }
    )]
    public static void TryParseCommandInputType_Enum_Valid(
        string typeDefinition,
        string[] expectedKeys,
        int[] expectedValues
    )
    {
        _ = Parser.TryParseCommandInputType(typeDefinition, out var result).ConfirmTrue();

        _ = result.Type.ConfirmEqual(ECommandInputType.Enum);
        _ = result.IsArray.ConfirmFalse();

        CommandTypeEnum e = result.ConfirmType<CommandTypeEnum>();
        _ = e.Values.Keys.ConfirmElementsAreEquivalent(expectedKeys);
        _ = e.Values.Values.ConfirmElementsAreEquivalent(expectedValues);
    }

    [TestCase("enum")]
    [TestCase("enum()")]
    [TestCase("enum(A)")]
    [TestCase("enum(A,)")]
    [TestCase("enum(A:1, B:: 2)")]
    [TestCase("enum(A:1, B:: B)")]
    [TestCase("enum(A:1, B:: C)")]
    public static void TryParseCommandInputType_Enum_Invalid(string typeDefinition)
    {
        _ = Parser.TryParseCommandInputType(typeDefinition, out _).ConfirmFalse();
    }
    #endregion TryParseCommandInputType_Enum

    #region TryParseEnumType
    [TestCase(
        new string[] { "A:1", "B:2", "C:3" },
        new string[] { "A", "B", "C" },
        new int[] { 1, 2, 3 }
    )]
    [TestCase(
        new string[] { "d:1", "E:2", "f:3" },
        new string[] { "d", "E", "f" },
        new int[] { 1, 2, 3 }
    )]
    [TestCase(
        new string[] { "G:-5", "H:13", "I:3" },
        new string[] { "G", "H", "I" },
        new int[] { -5, 13, 3 }
    )]
    public static void TryParseEnumType_Valid(
        string[] tokens,
        string[] expectedKeys,
        int[] expectedValues
    )
    {
        _ = Parser.TryParseEnumType(tokens, false, out var result).ConfirmTrue();

        CommandTypeEnum r = result.ConfirmNotNull().ConfirmType<CommandTypeEnum>();
        _ = r.Values.Keys.ConfirmElementsAreEquivalent(expectedKeys);
        _ = r.Values.Values.ConfirmElementsAreEquivalent(expectedValues);
    }

    [TestCase(new string[] { "A:1", "B::2" }, 0)]
    [TestCase(new string[] { "A:A" }, 0)]
    [TestCase(new string[] { ":A" }, 0)]
    [TestCase(new string[] { "1:A" }, 0)]
    [TestCase(new string[] { "A:" }, 0)]
    [TestCase(new string[] { ":" }, 0)]
    [TestCase(new string[] { }, 0)]
    public static void TryParseEnumType_InvalidDefinition(string[] tokens, int _)
    {
        Parser.TryParseEnumType(tokens, false, out var result).ConfirmFalse();
        result.ConfirmNull();
    }
    #endregion TryParseEnumType

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
    [TestCase(ECommandInputType.String, "-5:15", ECommandInputType.String, 0, 0, false, false)]
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
        _ = Parser.TryParseTypeWithRange(type, range, isArray, out var result).ConfirmEqual(isSuccess);

        if (!isSuccess)
        {
            return;
        }

        _ = result.Type.ConfirmEqual(eType);
        _ = result.Min.ConfirmEqual(eMin);
        _ = result.Max.ConfirmEqual(eMax);
        _ = result.IsArray.ConfirmEqual(isArray);
    }

    [TestCase("Void", ECommandInputType.Void, true)]
    [TestCase("void", ECommandInputType.Void, true)]
    [TestCase("int", Int, true)]
    [TestCase("enum?", ECommandInputType.Void, false)]
    [TestCase("enum", ECommandInputType.Enum, true)]
    public static void TryParseStringTypeToEnum(string type, ECommandInputType expected, bool shouldBeSuccess)
    {
        bool isSuccess = Parser.TryParseStringTypeToEnum(type, out var result);

        _ = isSuccess.ConfirmEqual(shouldBeSuccess);
        _ = result.ConfirmEqual(expected);
    }

    #region TryConvertStringToType_String
    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_String_Valid()
    {
        string str = _rg.NextString(8, 12);

        _ = Parser.TryConvertStringToType(
            str,
            new CommandTypeRanged(
                _rg.NextString(8, 12),
                ECommandInputType.String,
                false
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.Success);
        _ = result.ConfirmType<string>().ConfirmEqual(str);
    }

    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_String_InRange()
    {
        var (min, max) = (_rg.Next(16, 32), _rg.Next(32, 64));
        string str = _rg.NextString(min, max);

        _ = Parser.TryConvertStringToType(
            str,
            new CommandTypeRanged(
                _rg.NextString(8, 12),
                ECommandInputType.String,
                false,
                min,
                max
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.Success);
        _ = result.ConfirmType<string>().ConfirmHasLength(str.Length);
    }

    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_String_OutOfRange()
    {
        var (min, max) = (_rg.Next(16, 32), _rg.Next(32, 64));
        string str = _rg.NextBool()
            ? _rg.NextString(1, min)
            : _rg.NextString(max + 1, max * 2);

        _ = Parser.TryConvertStringToType(
            str,
            new CommandTypeRanged(
                _rg.NextString(8, 12),
                ECommandInputType.String,
                false,
                min,
                max
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.OutOfRange);
        _ = result.ConfirmNull();
    }
    #endregion TryConvertStringToType_String

    #region TryConvertStringToType_Int
    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_Int_Valid()
    {
        int num = (int)_rg.NextDouble(float.MinValue, float.MaxValue);

        _ = Parser.TryConvertStringToType(
            num.ToString(),
            new CommandTypeRanged(
                _rg.NextString(8, 12),
                Int,
                false
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.Success);
        _ = result.ConfirmType<int>().ConfirmEqual(num);
    }

    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_Int_Invalid()
    {
        _ = Parser.TryConvertStringToType(
            _rg.NextString(8, 12),
            new CommandTypeRanged(
                _rg.NextString(8, 12),
                Int,
                false
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.Invalid);
        _ = result.ConfirmNull();
    }

    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_Int_InRange()
    {
        var (min, max) = (_rg.Next(16, 32), _rg.Next(32, 64));
        int num = _rg.Next(min, max);

        _ = Parser.TryConvertStringToType(
            num.ToString(),
            new CommandTypeRanged(
                _rg.NextString(8, 12),
                Int,
                false,
                min,
                max
            ),
            out var result
        ).ConfirmEqual(
            EStringConversionResult.Success,
            $"{num} was outside of {min}-{max}"
        );
        _ = result.ConfirmType<int>().ConfirmEqual(num);
    }

    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_Int_OutsideOfRange()
    {
        var (min, max) = (_rg.Next(16, 32), _rg.Next(32, 64));
        int num = _rg.NextBool()
            ? _rg.Next(1, min)
            : _rg.Next(max + 1, max * 2);

        _ = Parser.TryConvertStringToType(
            num.ToString(),
            new CommandTypeRanged(
                _rg.NextString(8, 12),
                Int,
                false,
                min,
                max
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.OutOfRange);
        _ = result.ConfirmNull();
    }
    #endregion TryConvertStringToType_Int

    #region TryConvertStringToType_Float
    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_Float_Valid()
    {
        float num = (float)_rg.NextDouble(float.MinValue, float.MaxValue);

        _ = Parser.TryConvertStringToType(
            num.ToString(),
            new CommandTypeRanged(
                _rg.NextString(8, 12),
                Float,
                false
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.Success);
        _ = result.ConfirmType<float>().ConfirmEqual(num);
    }

    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_Float_Invalid()
    {
        _ = Parser.TryConvertStringToType(
            _rg.NextString(8, 12),
            new CommandTypeRanged(
                _rg.NextString(8, 12),
                Float,
                false
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.Invalid);
        _ = result.ConfirmNull();
    }

    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_Float_InRange()
    {
        var (min, max) = (_rg.NextDouble(16, 32), _rg.NextDouble(32, 64));
        float num = (float)_rg.NextDouble(min, max);

        _ = Parser.TryConvertStringToType(
            num.ToString(),
            new CommandTypeRanged(
                _rg.NextString(8, 12),
                Float,
                false,
                (float)min,
                (float)max
            ),
            out var result
        ).ConfirmEqual(
            EStringConversionResult.Success,
            $"{num} was outside of {min}-{max}"
        );
        _ = result.ConfirmType<float>().ConfirmEqual(num);
    }

    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_Float_OutsideOfRange()
    {
        var (min, max) = (_rg.NextDouble(16, 32), _rg.NextDouble(32, 64));
        float num = (float)(_rg.NextBool()
            ? _rg.NextDouble(1, min)
            : _rg.NextDouble(max + 1, max * 2));

        _ = Parser.TryConvertStringToType(
            num.ToString(),
            new CommandTypeRanged(
                _rg.NextString(8, 12),
                Float,
                false,
                (float)min,
                (float)max
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.OutOfRange);
        _ = result.ConfirmNull();
    }
    #endregion TryConvertStringToType_Float

    #region TryConvertStringToType_Bool
    [TestCase(true)]
    [TestCase(false)]
    public static void TryConvertStringToType_Bool_Valid(bool boolean)
    {
        _ = Parser.TryConvertStringToType(
            boolean.ToString(),
            new(
                _rg.NextString(8, 12),
                Bool,
                false
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.Success);
        _ = result.ConfirmType<bool>().ConfirmEqual(boolean);
    }

    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_Bool_Invalid()
    {
        _ = Parser.TryConvertStringToType(
            _rg.NextString(8, 12),
            new(
                _rg.NextString(8, 12),
                Bool,
                false
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.Invalid);
        _ = result.ConfirmNull();
    }
    #endregion TryConvertStringToType_Bool

    #region TryConvertStringToType_Constant
    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_Constant_Valid()
    {
        string constant = _rg.NextString(8, 12);

        _ = Parser.TryConvertStringToType(
            constant,
            new(
                constant,
                Constant,
                false
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.Success);
        _ = result.ConfirmType<string>().ConfirmEqual(constant);
    }

    [Repeat(5)]
    [TestCase]
    public static void TryConvertStringToType_Constant_Invalid()
    {
        _ = Parser.TryConvertStringToType(
            _rg.NextString(8, 12),
            new(
                _rg.NextString(8, 12),
                Constant,
                false
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.Invalid);
        _ = result.ConfirmNull();
    }
    #endregion TryConvertStringToType_Constant

    [TestCase]
    public static void TryConvertStringToType_Void()
    {
        _ = Parser.TryConvertStringToType(
            _rg.NextString(8, 12),
            new(
                _rg.NextString(8, 12),
                ECommandInputType.Void,
                false
            ),
            out var result
        ).ConfirmEqual(EStringConversionResult.Invalid);
        _ = result.ConfirmNull();
    }
}
