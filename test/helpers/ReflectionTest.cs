using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Confirma.Attributes;
using Confirma.Extensions;
using Godot;
using YAT.Attributes;
using YAT.Commands;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class ReflectionTest
{
    private static Button? _button;

    private interface ITestInterface { }

    [Command("test")]
    [Argument("arg1", "string")]
    [Argument("arg3", "string")]
    [Argument("arg3", "string")]
    private class TestClass { }

    private class TestClassWithInterface : ITestInterface { }

    [Extension("test", "Test extension.", "Test extension manual.", "test_alias", "test_alias2")]
    private class TestExtension : IExtension
    {
        public CommandResult Execute(CommandData _)
        {
            return ICommand.NotImplemented();
        }
    }

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
        IEnumerable<EventInfo> events = _button!.GetEvents(
            BindingFlags.Instance | BindingFlags.Public
        );

        _ = events.ConfirmNotNull();
        _ = events.Count().ConfirmIsPositive();
    }

    [TestCase]
    public static void GetEvents_NoEvents()
    {
        IEnumerable<EventInfo> events = Reflection.GetEvents(new TestClass());

        _ = events.ConfirmNotNull();
        _ = events.Any().ConfirmFalse();
    }
    #endregion GetEvents

    #region GetAttribute
    [TestCase]
    public static void GetAttribute_AttributePresent()
    {
        CommandAttribute? attribute = Reflection.GetAttribute<CommandAttribute>(new TestClass());

        _ = attribute.ConfirmNotNull();
        _ = (attribute?.Name.ConfirmEqual("test"));
    }

    [TestCase]
    public static void GetAttribute_AttributeNotPresent()
    {
        OptionAttribute? attribute = Reflection.GetAttribute<OptionAttribute>(new TestClass());

        _ = attribute.ConfirmNull();
    }

    [TestCase]
    public static void GetAttribute_AttributesPresent()
    {
        IEnumerable<ArgumentAttribute>? attributes = Reflection.GetAttributes<ArgumentAttribute>(new TestClass());

        _ = attributes.ConfirmNotNull();
        _ = (attributes?.Count().ConfirmEqual(3));
        _ = (attributes?.ElementAt(0).Name.ConfirmEqual("arg1"));
        _ = (attributes?.ElementAt(1).Name.ConfirmEqual("arg3"));
        _ = (attributes?.ElementAt(2).Name.ConfirmEqual("arg3"));
    }

    [TestCase]
    public static void GetAttribute_AttributesNotPresent()
    {
        IEnumerable<OptionAttribute>? attributes = Reflection.GetAttributes<OptionAttribute>(new TestClass());

        _ = (attributes?.ConfirmEmpty());
    }
    #endregion GetAttribute

    #region HasAttribute
    [TestCase]
    public static void HasAttribute_AttributePresent()
    {
        bool hasAttribute = Reflection.HasAttribute<CommandAttribute>(new TestClass());

        _ = hasAttribute.ConfirmTrue();
    }

    [TestCase]
    public static void HasAttribute_AttributeNotPresent()
    {
        bool hasAttribute = Reflection.HasAttribute<OptionAttribute>(new TestClass());

        _ = hasAttribute.ConfirmFalse();
    }
    #endregion HasAttribute

    #region HasInterface
    [TestCase]
    public static void HasInterface_InterfacePresent()
    {
        _ = new Cls().HasInterface<ICommand>().ConfirmTrue();
    }

    [TestCase]
    public static void HasInterface_InterfacePresent_Type()
    {
        _ = typeof(TestExtension).HasInterface<IExtension>().ConfirmTrue();
    }

    [TestCase]
    public static void HasInterface_InterfaceNotPresent()
    {
        _ = new TestClass().HasInterface<ITestInterface>().ConfirmFalse();
    }
    #endregion HasInterface
}
