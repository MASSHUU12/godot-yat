using Chickensoft.GoDotTest;
using Godot;
using YAT.Attributes;
using YAT.Helpers;
using Shouldly;

namespace Test;

public class TestReflection : TestClass
{
	public TestReflection(Node testScene) : base(testScene) { }

	[Command("test")]
	[Argument("arg1", "string")]
	[Argument("arg3", "string")]
	[Argument("arg3", "string")]
	private class TestClass { }

	[Test]
	public void TestGetAttribute_AttributePresent()
	{
		var obj = new TestClass();
		var attribute = Reflection.GetAttribute<CommandAttribute>(obj);

		attribute.ShouldNotBeNull();
		attribute.Name.ShouldBe("test");
	}

	[Test]
	public void TestGetAttribute_AttributeNotPresent()
	{
		var obj = new TestClass();
		var attribute = Reflection.GetAttribute<OptionAttribute>(obj);

		attribute.ShouldBeNull();
	}

	[Test]
	public void TestGetAttribute_AttributesPresent()
	{
		var obj = new TestClass();
		var attributes = Reflection.GetAttributes<ArgumentAttribute>(obj);

		attributes.ShouldNotBeNull();
		attributes.Length.ShouldBe(3);
		attributes[0].Name.ToString().ShouldBe("arg1");
		attributes[1].Name.ToString().ShouldBe("arg3");
		attributes[2].Name.ToString().ShouldBe("arg3");
	}

	[Test]
	public void TestGetAttribute_AttributesNotPresent()
	{
		var obj = new TestClass();
		var attributes = Reflection.GetAttributes<OptionAttribute>(obj);

		attributes.ShouldBeEmpty();
	}

	[Test]
	public void TestHasAttribute_AttributePresent()
	{
		var obj = new TestClass();
		var hasAttribute = Reflection.HasAttribute<CommandAttribute>(obj);

		hasAttribute.ShouldBeTrue();
	}

	[Test]
	public void TestHasAttribute_AttributeNotPresent()
	{
		var obj = new TestClass();
		var hasAttribute = Reflection.HasAttribute<OptionAttribute>(obj);

		hasAttribute.ShouldBeFalse();
	}
}
