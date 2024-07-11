using System.Collections.Generic;
using Confirma.Attributes;
using Confirma.Extensions;
using YAT.Classes;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class CommandTypeEnumTest
{
    [TestCase]
    public static void GenerateTypeDefinition_ReturnsCorrectString()
    {
        Dictionary<string, int> values = new()
        {
                { "Value1", 1 },
                { "Value2", 2 },
                { "Value3", 3 }
            };

        CommandTypeEnum commandTypeEnum = new("MyEnum", false, values);

        _ = commandTypeEnum.TypeDefinition.ConfirmEqual("Value1(1), Value2(2), Value3(3)");
    }
}
