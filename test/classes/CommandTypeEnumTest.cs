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
        var values = new Dictionary<string, int>
            {
                { "Value1", 1 },
                { "Value2", 2 },
                { "Value3", 3 }
            };

        var commandTypeEnum = new CommandTypeEnum("MyEnum", false, values);

        commandTypeEnum.TypeDefinition.ConfirmEqual("Value1(1), Value2(2), Value3(3)");
    }
}
