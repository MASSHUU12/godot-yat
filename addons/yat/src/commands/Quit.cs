using YAT.Attributes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("quit", "By default quits the game.", aliases: "exit")]
[Option("-t", "bool", "Closes the terminal.")]
public sealed class Quit : ICommand
{
#nullable disable
    private YAT _yat;
    private BaseTerminal _terminal;
#nullable restore

    public CommandResult Execute(CommandData data)
    {
        var t = (bool)data.Options["-t"];

        _yat = data.Yat;
        _terminal = data.Terminal;

        if (t)
        {
            CloseTerminal();
        }
        else
        {
            QuitTheGame();
        }

        return ICommand.Success();
    }

    private void CloseTerminal()
    {
        _ = _yat.CallDeferred("CloseTerminal");
    }

    private void QuitTheGame()
    {
        _terminal.Print("Quitting...");
        _yat.GetTree().Quit();
    }
}
