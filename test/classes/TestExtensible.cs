namespace GdUnit4
{
	using System;
	using YAT.Attributes;
	using YAT.Classes;
	using YAT.Enums;
	using YAT.Interfaces;
	using YAT.Types;
	using static Assertions;

	[TestSuite]
	public partial class TestExtensible
	{
		[Extension("test", "Test extension.", "Test extension manual.", "test_alias", "test_alias2")]
		private class TestExtension : IExtension
		{
			public CommandResult Execute(CommandData data)
			{
				return CommandResult.NotImplemented;
			}
		}

		private class TestExtensionWithoutAttribute : IExtension
		{
			public CommandResult Execute(CommandData data)
			{
				return CommandResult.NotImplemented;
			}
		}

		[TestCase("test_command", typeof(TestExtension), true)]
		[TestCase("test_command", typeof(TestExtension), false)]
		[TestCase("test_command", typeof(TestExtensionWithoutAttribute), false)]
		public void TestRegisterExtension(string commandName, Type extensionType, bool expected)
		{
			AssertBool(Extensible.RegisterExtension(commandName, extensionType)).IsEqual(expected);
		}

		public void TestUnregisterExtensionExtensionPresent()
		{
			Extensible.RegisterExtension("test_command", typeof(TestExtension));

			AssertBool(Extensible.UnregisterExtension("test_command", typeof(TestExtension))).IsTrue();
		}

		[TestCase("test_command", typeof(TestExtension), false)]
		[TestCase("test_command", typeof(TestExtensionWithoutAttribute), false)]
		public void TestUnregisterExtension(string commandName, Type extensionType, bool expected)
		{
			AssertBool(Extensible.UnregisterExtension(commandName, extensionType)).IsEqual(expected);
		}

		// TODO: Test ExecuteExtension
		// TODO: Test GenerateExtensionManual
	}
}
