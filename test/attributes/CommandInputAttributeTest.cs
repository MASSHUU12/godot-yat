using System;
using Confirma.Attributes;
using Confirma.Classes;
using Confirma.Extensions;
using YAT.Attributes;
using YAT.Enums;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class CommandInputAttributeTest
{
    private static readonly Random _rg = new();

    [Repeat(5)]
    [TestCase]
    public static void Constructor_SetCorrectly_OneType()
    {
        string name = _rg.NextString(8, 12);
        string description = _rg.NextString(16, 32);
        string type = _rg.NextElement(new string[] { "int", "float", "string", "bool" });

        CommandInputAttribute attribute = new(name, type, description);

        _ = attribute.Name.ConfirmEqual(name);
        _ = attribute.Description.ConfirmEqual(description);
        _ = attribute.Types.ConfirmCount(1);
    }

    [TestCase]
    public static void Constructor_TypeIsNull_ThrowsArgumentNullException()
    {
        const string name = "TestName";
        const string description = "TestDescription";
        const string? type = null;

        _ = Confirm.Throws<ArgumentNullException>(
            () => _ = new CommandInputAttribute(name, type!, description)
        );
    }

    [TestCase]
    public static void Constructor_TypeIsEmptyString_ThrowsArgumentNullException()
    {
        const string name = "TestName";
        const string description = "TestDescription";
        string type = string.Empty;

        _ = Confirm.Throws<ArgumentNullException>(
            () => _ = new CommandInputAttribute(name, type, description)
        );
    }

    [TestCase]
    public static void Constructor_TypesAreValid_ParsesCorrectly()
    {
        const string name = "TestName";
        const string description = "TestDescription";
        const string type = "TestType1|TestType2";

        CommandInputAttribute attribute = new(name, type, description);

        _ = attribute.Types.ConfirmCount(2);
        _ = attribute.Types.ConfirmAllMatch((item) => item.Type == ECommandInputType.Constant);
    }

    [TestCase("string(:ddd)")]
    [TestCase("string(ddd:)")]
    [TestCase("string(5:1)")]
    [TestCase("string(-5:15)")]
    [TestCase("string(5:-15)")]
    public static void Constructor_TypeIsInvalid_ThrowsArgumentException(string type)
    {
        const string name = "TestName";
        const string description = "TestDescription";

        _ = Confirm.Throws<ArgumentException>(
            () => _ = new CommandInputAttribute(name, type, description)
        );
    }

    [TestCase()]
    public static void Constructor_TypeIsVoid_ThrowsArgumentException()
    {
        const string name = "TestName";
        const string description = "TestDescription";

        _ = Confirm.Throws<ArgumentException>(
            () => _ = new CommandInputAttribute(name, "void", description)
        );
    }
}
