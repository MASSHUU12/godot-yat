using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("cls", "Clears the console.", aliases: "clear")]
public sealed class Cls : ICommand
{
    public string[]? Arguments { get; set; }

    public CommandResult Execute(CommandData data)
    {
        data.Terminal.Clear();

        return ICommand.Success();
    }
}
