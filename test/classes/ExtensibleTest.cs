using System;
using Confirma.Attributes;
using Confirma.Extensions;
using YAT.Attributes;
using YAT.Classes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class ExtensibleTest
{
    [Extension("test", "Test extension.", "Test extension manual.", "test_alias", "test_alias2")]
    private class TestExtension : IExtension
    {
        public CommandResult Execute(CommandData data)
        {
            return ICommand.NotImplemented();
        }
    }

    private class TestExtensionWithoutAttribute : IExtension
    {
        public CommandResult Execute(CommandData data)
        {
            return ICommand.NotImplemented();
        }
    }

    [TestCase("test_command", typeof(TestExtension), true)]
    [TestCase("test_command", typeof(TestExtension), false)]
    [TestCase("test_command", typeof(TestExtensionWithoutAttribute), false)]
    public static void TestRegisterExtension(string commandName, Type extensionType, bool expected)
    {
        _ = Extensible.RegisterExtension(commandName, extensionType).ConfirmEqual(expected);
    }

    public static void TestUnregisterExtensionExtensionPresent()
    {
        _ = Extensible.RegisterExtension("test_command", typeof(TestExtension));
        _ = Extensible.UnregisterExtension("test_command", typeof(TestExtension)).ConfirmTrue();
    }

    [TestCase("test_command", typeof(TestExtension), false)]
    [TestCase("test_command", typeof(TestExtensionWithoutAttribute), false)]
    public static void TestUnregisterExtension(string commandName, Type extensionType, bool expected)
    {
        _ = Extensible.UnregisterExtension(commandName, extensionType).ConfirmEqual(expected);
    }
}
