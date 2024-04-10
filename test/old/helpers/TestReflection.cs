using Chickensoft.GoDotTest;
using Godot;
using YAT.Attributes;
using YAT.Helpers;
using Shouldly;
using YAT.Commands;
using YAT.Interfaces;

namespace Test;

public class TestReflection : TestClass
{
	public TestReflection(Node testScene) : base(testScene) { }

	private interface ITestInterface { }

	[Command("test")]
	[Argument("arg1", "string")]
	[Argument("arg3", "string")]
	[Argument("arg3", "string")]
	private class TestClass { }

	private class TestClassWithInterface : ITestInterface { }

	[Test]
	public static void TestGetAttribute_AttributePresent()
	{
		var attribute = Reflection.GetAttribute<CommandAttribute>(new TestClass());

		attribute.ShouldNotBeNull();
		attribute.Name.ShouldBe("test");
	}

	[Test]
	public static void TestGetAttribute_AttributeNotPresent()
	{
		var attribute = Reflection.GetAttribute<OptionAttribute>(new TestClass());

		attribute.ShouldBeNull();
	}

	[Test]
	public static void TestGetAttribute_AttributesPresent()
	{
		var attributes = Reflection.GetAttributes<ArgumentAttribute>(new TestClass());

		attributes.ShouldNotBeNull();
		attributes.Length.ShouldBe(3);
		attributes[0].Name.ToString().ShouldBe("arg1");
		attributes[1].Name.ToString().ShouldBe("arg3");
		attributes[2].Name.ToString().ShouldBe("arg3");
	}

	[Test]
	public static void TestGetAttribute_AttributesNotPresent()
	{
		var attributes = Reflection.GetAttributes<OptionAttribute>(new TestClass());

		attributes.ShouldBeEmpty();
	}

	[Test]
	public static void TestHasAttribute_AttributePresent()
	{
		var hasAttribute = Reflection.HasAttribute<CommandAttribute>(new TestClass());

		hasAttribute.ShouldBeTrue();
	}

	[Test]
	public static void TestHasAttribute_AttributeNotPresent()
	{
		var hasAttribute = Reflection.HasAttribute<OptionAttribute>(new TestClass());

		hasAttribute.ShouldBeFalse();
	}

	[Test]
	public static void TestHasInterface_InterfacePresent()
	{
		new Cls().HasInterface<ICommand>().ShouldBeTrue();
	}

	[Test]
	public static void TestHasInterface_InterfaceAbsent()
	{
		new TestClass().HasInterface<ITestInterface>().ShouldBeFalse();
	}
}
