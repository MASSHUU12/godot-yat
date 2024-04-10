using Chickensoft.GoDotTest;
using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;
using Shouldly;
using YAT.Enums;

namespace Test;

public class TestRegisteredCommands : TestClass
{
	public TestRegisteredCommands(Node testScene) : base(testScene) { }

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

	[Test]
	public static void TestAddCommand()
	{
		RegisteredCommands.AddCommand(typeof(TestCommand)).ShouldBe(ECommandAdditionStatus.Success);
		RegisteredCommands.AddCommand(typeof(TestCommand)).ShouldBe(ECommandAdditionStatus.ExistentCommand);
		RegisteredCommands.AddCommand(typeof(TestCommandWithoutAttribute)).ShouldBe(ECommandAdditionStatus.MissingAttribute);
		RegisteredCommands.AddCommand(typeof(TestCommandWithoutInterface)).ShouldBe(ECommandAdditionStatus.UnknownCommand);
	}
}
