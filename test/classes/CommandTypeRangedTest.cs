using Confirma.Attributes;
using Confirma.Extensions;
using YAT.Classes;
using static YAT.Enums.ECommandInputType;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class CommandTypeRangedTest
{
    [TestCase("", 0f, 0f, "(0:0)")]
    [TestCase("range", float.MinValue, 5.5f, "range(:5.5)")]
    [TestCase("number", -69f, float.MaxValue, "number(-69:)")]
    public static void TestGetTypeDefinition(
        string name,
        float min,
        float max,
        string expectedDefinition
    )
    {
        CommandTypeRanged commandType = new(name, Void, false, min, max);

        commandType.TypeDefinition.ConfirmEqual(expectedDefinition);
    }
}
