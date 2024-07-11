using Confirma.Attributes;
using Confirma.Extensions;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public static class RegisteredCommandsTest
{
    [Command("test")]
    private class TestCommand : ICommand
    {
        public CommandResult Execute(CommandData data)
        {
            return ICommand.NotImplemented();
        }
    }

    private class TestCommandWithoutAttribute : ICommand
    {
        public CommandResult Execute(CommandData data)
        {
            return ICommand.NotImplemented();
        }
    }

    [Command("test")]
    private static class TestCommandWithoutInterface
    {
        public static CommandResult Execute(CommandData data)
        {
            return ICommand.NotImplemented();
        }
    }

    public static void TestAddCommand()
    {
        _ = RegisteredCommands
            .AddCommand(typeof(TestCommand))
            .ConfirmEqual(ECommandAdditionStatus.Success);
        _ = RegisteredCommands
            .AddCommand(typeof(TestCommand))
            .ConfirmEqual(ECommandAdditionStatus.ExistentCommand);
        _ = RegisteredCommands
            .AddCommand(typeof(TestCommandWithoutAttribute))
            .ConfirmEqual(ECommandAdditionStatus.MissingAttribute);
        _ = RegisteredCommands
            .AddCommand(typeof(TestCommandWithoutInterface))
            .ConfirmEqual(ECommandAdditionStatus.UnknownCommand);
    }
}
