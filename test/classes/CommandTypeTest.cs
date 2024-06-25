using Confirma.Attributes;
using Confirma.Extensions;
using YAT.Classes;
using YAT.Enums;
using static YAT.Enums.ECommandInputType;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class CommandTypeTest
{
	[TestCase("", Void, false, "")]
	[TestCase("int", Int, false, "int")]
	[TestCase("float", Float, true, "float")]
	[TestCase("Lorem", Constant, false, "Lorem")]
	[TestCase("choice", Constant, true, "choice")]
	public static void TestGetTypeDefinition(
		string name,
		ECommandInputType type,
		bool isArray,
		string expectedDefinition
	)
	{
		CommandType commandType = new(name, type, isArray);

		commandType.GetTypeDefinition().ConfirmEqual(expectedDefinition);
	}
}
