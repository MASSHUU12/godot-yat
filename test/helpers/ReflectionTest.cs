using System.Reflection;
using Confirma.Attributes;
using Confirma.Extensions;
using Godot;
using YAT.Attributes;
using YAT.Commands;
using YAT.Helpers;
using YAT.Interfaces;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class ReflectionTest
{
	private static Button? _button = null;

	private interface ITestInterface { }

	[Command("test")]
	[Argument("arg1", "string")]
	[Argument("arg3", "string")]
	[Argument("arg3", "string")]
	private class TestClass { }

	private class TestClassWithInterface : ITestInterface { }

	[SetUp]
	public static void SetUp()
	{
		_button = new();
	}

	[TearDown]
	public static void TearDown()
	{
		_button!.QueueFree();
	}

	#region GetEvents
	[TestCase]
	public static void GetEvents_EventsPresent()
	{
		var events = _button!.GetEvents(
			BindingFlags.Instance
			| BindingFlags.Public
		);

		GD.Print(events.Length, events);

		events.ConfirmNotNull();
		events.Length.ConfirmIsPositive(events.Length.ToString());
	}

	[TestCase]
	public static void GetEvents_NoEvents()
	{
		var events = Reflection.GetEvents(new TestClass());

		events.ConfirmNotNull();
		events.Length.ConfirmEqual(0);
	}
	#endregion

	#region GetAttribute
	[TestCase]
	public static void GetAttribute_AttributePresent()
	{
		var attribute = Reflection.GetAttribute<CommandAttribute>(new TestClass());

		attribute.ConfirmNotNull();
		attribute?.Name.ConfirmEqual("test");
	}

	[TestCase]
	public static void GetAttribute_AttributeNotPresent()
	{
		var attribute = Reflection.GetAttribute<OptionAttribute>(new TestClass());

		attribute.ConfirmNull();
	}

	[TestCase]
	public static void GetAttribute_AttributesPresent()
	{
		var attributes = Reflection.GetAttributes<ArgumentAttribute>(new TestClass());

		attributes.ConfirmNotNull();
		attributes?.Length.ConfirmEqual(3);
		attributes?[0].Name.ToString().ConfirmEqual("arg1");
		attributes?[1].Name.ToString().ConfirmEqual("arg3");
		attributes?[2].Name.ToString().ConfirmEqual("arg3");
	}

	[TestCase]
	public static void GetAttribute_AttributesNotPresent()
	{
		var attributes = Reflection.GetAttributes<OptionAttribute>(new TestClass());

		attributes?.ConfirmEmpty();
	}
	#endregion

	#region HasAttribute
	[TestCase]
	public static void HasAttribute_AttributePresent()
	{
		var hasAttribute = Reflection.HasAttribute<CommandAttribute>(new TestClass());

		hasAttribute.ConfirmTrue();
	}

	[TestCase]
	public static void HasAttribute_AttributeNotPresent()
	{
		var hasAttribute = Reflection.HasAttribute<OptionAttribute>(new TestClass());

		hasAttribute.ConfirmFalse();
	}
	#endregion

	#region HasInterface
	[TestCase]
	public static void HasInterface_InterfacePresent()
	{
		new Cls().HasInterface<ICommand>().ConfirmTrue();
	}

	[TestCase]
	public static void HasInterface_InterfaceNotPresent()
	{
		new TestClass().HasInterface<ITestInterface>().ConfirmFalse();
	}
	#endregion
}
