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
public static class TestExtensible
{
	[Extension("test", "Test extension.", "Test extension manual.", "test_alias", "test_alias2")]
	private class TestExtension : IExtension
	{
		public CommandResult Execute(CommandData data) => ICommand.NotImplemented();
	}

	private class TestExtensionWithoutAttribute : IExtension
	{
		public CommandResult Execute(CommandData data) => ICommand.NotImplemented();
	}

	[TestCase("test_command", typeof(TestExtension), true)]
	[TestCase("test_command", typeof(TestExtension), false)]
	[TestCase("test_command", typeof(TestExtensionWithoutAttribute), false)]
	public static void TestRegisterExtension(string commandName, Type extensionType, bool expected)
	{
		Extensible.RegisterExtension(commandName, extensionType).ConfirmEqual(expected);
	}

	public static void TestUnregisterExtensionExtensionPresent()
	{
		Extensible.RegisterExtension("test_command", typeof(TestExtension));

		Extensible.UnregisterExtension("test_command", typeof(TestExtension)).ConfirmTrue();
	}

	[TestCase("test_command", typeof(TestExtension), false)]
	[TestCase("test_command", typeof(TestExtensionWithoutAttribute), false)]
	public static void TestUnregisterExtension(string commandName, Type extensionType, bool expected)
	{
		Extensible.UnregisterExtension(commandName, extensionType).ConfirmEqual(expected);
	}
}
