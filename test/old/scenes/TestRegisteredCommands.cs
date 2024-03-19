using System;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Types;

namespace GdUnit4
{
	using static Assertions;
	using static YAT.Scenes.RegisteredCommands;

	[TestSuite]
	public partial class TestRegisteredCommands
	{
		[Command("test")]
		private class TestCommand : ICommand
		{
			public CommandResult Execute(CommandData data) => ICommand.NotImplemented();
		}

		private class TestCommandWithoutAttribute : ICommand
		{
			public CommandResult Execute(CommandData data) => ICommand.NotImplemented();
		}

		[Command("test")]
		private class TestCommandWithoutInterface
		{
			public CommandResult Execute(CommandData data) => ICommand.NotImplemented();
		}

		[TestCase(typeof(TestCommand), ECommandAdditionStatus.Success)]
		[TestCase(typeof(TestCommand), ECommandAdditionStatus.ExistentCommand)]
		[TestCase(typeof(TestCommandWithoutAttribute), ECommandAdditionStatus.MissingAttribute)]
		[TestCase(typeof(TestCommandWithoutInterface), ECommandAdditionStatus.UnknownCommand)]
		public void TestAddCommand(Type commandType, ECommandAdditionStatus expected)
		{
			AssertObject(AddCommand(commandType)).IsEqual(expected);
		}
	}
}
