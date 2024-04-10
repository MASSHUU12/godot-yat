using Confirma;
using Confirma.Attributes;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Test;

[TestClass]
public static class TestRegisteredCommands
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
		public static CommandResult Execute(CommandData data) => ICommand.NotImplemented();
	}

	public static void TestAddCommand()
	{
		RegisteredCommands.AddCommand(typeof(TestCommand)).ConfirmEqual(ECommandAdditionStatus.Success);
		RegisteredCommands.AddCommand(typeof(TestCommand)).ConfirmEqual(ECommandAdditionStatus.ExistentCommand);
		RegisteredCommands.AddCommand(typeof(TestCommandWithoutAttribute)).ConfirmEqual(ECommandAdditionStatus.MissingAttribute);
		RegisteredCommands.AddCommand(typeof(TestCommandWithoutInterface)).ConfirmEqual(ECommandAdditionStatus.UnknownCommand);
	}
}
