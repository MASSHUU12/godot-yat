using System;
using Confirma.Attributes;
using Confirma.Extensions;
using YAT.Attributes;
using YAT.Scenes;

namespace YAT.Test.Scenes;

[AfterAll]
[TestClass]
[Parallelizable]
public static class CommandValidatorTest
{
    private static readonly CommandValidator _validator = new();
    private static readonly Random _rg = new();

    #region ValidateCommandArgument_String
    [Repeat(5)]
    [TestCase]
    public static void ValidateCommandArgument_String_Valid()
    {
        ArgumentAttribute argument = new(_rg.NextString(4, 12), "string");

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            _rg.NextString(8, 12),
            false
        ).ConfirmTrue();
    }

    [TestCase]
    public static void ValidateCommandArgument_String_Invalid()
    {
        ArgumentAttribute argument = new(_rg.NextString(4, 12), "string");

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            string.Empty,
            false
        ).ConfirmFalse("Empty string should be invalid.");
    }
    #endregion ValidateCommandArgument_String

    #region ValidateCommandArgument_Int
    [Repeat(5)]
    [TestCase]
    public static void ValidateCommandArgument_Int_Valid()
    {
        ArgumentAttribute argument = new(_rg.NextString(4, 12), "int");

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            _rg.Next(-256, 256).ToString(),
            false
        ).ConfirmTrue();
    }

    [Repeat(5)]
    [TestCase]
    public static void ValidateCommandArgument_Int_PassedString()
    {
        ArgumentAttribute argument = new(_rg.NextString(4, 12), "int");

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            _rg.NextString(),
            false
        ).ConfirmFalse("String is not a valid int.");
    }

    [Repeat(5)]
    [TestCase]
    public static void ValidateCommandArgument_IntRanged_Valid()
    {
        var (min, max) = (_rg.Next(32), _rg.Next(32, 64));
        ArgumentAttribute argument = new(
            _rg.NextString(4, 12),
            $"int({min}:{max})"
        );

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            _rg.Next(min, max).ToString(),
            false
        ).ConfirmTrue();
    }

    [Repeat(5)]
    [TestCase]
    public static void ValidateCommandArgument_IntRanged_OutsideOfRange()
    {
        var (min, max) = (_rg.Next(16, 32), _rg.Next(32, 64));
        ArgumentAttribute argument = new(
            _rg.NextString(4, 12),
            $"int({min}:{max})"
        );
        string generated = (_rg.NextBool()
            ? _rg.Next(-(min * 2), min - 1)
            : _rg.Next(max + 1, max * 2)).ToString();

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            generated,
            false
        ).ConfirmFalse($"Expected {generated} to be outside of {min}-{max}.");
    }
    #endregion ValidateCommandArgument_Int

    #region ValidateCommandArgument_Float
    [Repeat(5)]
    [TestCase]
    public static void ValidateCommandArgument_Float_Valid()
    {
        ArgumentAttribute argument = new(_rg.NextString(4, 12), "float");

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            _rg.NextDouble(-256, 256).ToString(),
            false
        ).ConfirmTrue();
    }

    [Repeat(5)]
    [TestCase]
    public static void ValidateCommandArgument_Float_PassedString()
    {
        ArgumentAttribute argument = new(_rg.NextString(4, 12), "float");

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            _rg.NextString(),
            false
        ).ConfirmFalse("String is not a valid float.");
    }

    [Repeat(5)]
    [TestCase]
    public static void ValidateCommandArgument_FloatRanged_Valid()
    {
        var (min, max) = (_rg.NextDouble(0, 32), _rg.NextDouble(32, 64));
        ArgumentAttribute argument = new(
            _rg.NextString(4, 12),
            $"float({min}:{max})"
        );

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            _rg.NextDouble(min, max).ToString(),
            false
        ).ConfirmTrue();
    }

    [Repeat(5)]
    [TestCase]
    public static void ValidateCommandArgument_FloatRanged_OutsideOfRange()
    {
        var (min, max) = (_rg.NextDouble(0, 32), _rg.NextDouble(32, 64));
        ArgumentAttribute argument = new(
            _rg.NextString(4, 12),
            $"float({min}:{max})"
        );

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            _rg.NextDouble(min, max).ToString(),
            false
        ).ConfirmTrue();
    }
    #endregion ValidateCommandArgument_Float

    #region ValidateCommandArgument_Bool
    [Repeat(2)]
    [TestCase]
    public static void ValidateCommandArgument_Bool_Valid()
    {
        ArgumentAttribute argument = new(_rg.NextString(4, 12), "bool");

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            _rg.NextBool().ToString(),
            false
        ).ConfirmTrue();
    }

    [TestCase]
    public static void ValidateCommandArgument_Bool_PassedString()
    {
        ArgumentAttribute argument = new(_rg.NextString(4, 12), "bool");

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            _rg.NextString(),
            false
        ).ConfirmFalse("String is not a valid bool.");
    }
    #endregion ValidateCommandArgument_Bool

    #region ValidateCommandArgument_Constant
    [Repeat(5)]
    [TestCase]
    public static void ValidateCommandArgument_Constant_Valid()
    {
        string[] args = new string[] {
            _rg.NextString(4, 12),
            _rg.NextString(4, 12),
            _rg.NextString(4, 12)
        };
        ArgumentAttribute argument = new(
            _rg.NextString(4, 12),
            string.Join('|', args)
        );

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            _rg.NextElement(args),
            false
        ).ConfirmTrue();
    }

    [Repeat(5)]
    [TestCase]
    public static void ValidateCommandArgument_Constant_Invalid()
    {
        string[] args = new string[] {
            _rg.NextString(4, 12),
            _rg.NextString(4, 12),
            _rg.NextString(4, 12)
        };
        ArgumentAttribute argument = new(
            _rg.NextString(4, 12),
            string.Join('|', args)
        );

        _ = _validator.ValidateCommandArgument(
            argument,
            new(),
            _rg.NextString(13, 16),
            false
        ).ConfirmFalse();
    }
    #endregion ValidateCommandArgument_Constant

    // [Repeat(5)]
    // [TestCase]
    // public static void ValidateCommandArgument_ValidEnum()
    // {
    // TODO
    // }

    public static void AfterAll()
    {
        _validator.QueueFree();
    }
}
