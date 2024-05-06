using Confirma.Attributes;
using Confirma.Extensions;
using YAT.Attributes;
using YAT.Commands;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class ReflectionTest
{
	private interface ITestInterface { }

	[Command("test")]
	[Argument("arg1", "string")]
	[Argument("arg3", "string")]
	[Argument("arg3", "string")]
	private class TestClass { }

	private class TestClassWithInterface : ITestInterface { }

	[TestCase]
	public static void TestGetAttribute_AttributePresent()
	{
		var attribute = Reflection.GetAttribute<CommandAttribute>(new TestClass());

		attribute.ConfirmNotNull();
		attribute?.Name.ConfirmEqual("test");
	}

	[TestCase]
	public static void TestGetAttribute_AttributeNotPresent()
	{
		var attribute = Reflection.GetAttribute<OptionAttribute>(new TestClass());

		attribute.ConfirmNull();
	}

	[TestCase]
	public static void TestGetAttribute_AttributesPresent()
	{
		var attributes = Reflection.GetAttributes<ArgumentAttribute>(new TestClass());

		attributes.ConfirmNotNull();
		attributes?.Length.ConfirmEqual(3);
		attributes?[0].Name.ToString().ConfirmEqual("arg1");
		attributes?[1].Name.ToString().ConfirmEqual("arg3");
		attributes?[2].Name.ToString().ConfirmEqual("arg3");
	}

	[TestCase]
	public static void TestGetAttribute_AttributesNotPresent()
	{
		var attributes = Reflection.GetAttributes<OptionAttribute>(new TestClass());

		attributes?.ConfirmEmpty();
	}

	[TestCase]
	public static void TestHasAttribute_AttributePresent()
	{
		var hasAttribute = Reflection.HasAttribute<CommandAttribute>(new TestClass());

		hasAttribute.ConfirmTrue();
	}

	[TestCase]
	public static void TestHasAttribute_AttributeNotPresent()
	{
		var hasAttribute = Reflection.HasAttribute<OptionAttribute>(new TestClass());

		hasAttribute.ConfirmFalse();
	}

	[TestCase]
	public static void TestHasInterface_InterfacePresent()
	{
		new Cls().HasInterface<ICommand>().ConfirmTrue();
	}

	[TestCase]
	public static void TestHasInterface_InterfaceAbsent()
	{
		new TestClass().HasInterface<ITestInterface>().ConfirmFalse();
	}
}
