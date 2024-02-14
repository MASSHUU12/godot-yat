using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("quit", "By default quits the game.", "[b]Usage[/b]: quit", "exit")]
[Option("-t", null, "Closes the terminal.", false)]
public sealed class Quit : ICommand
{
	private YAT _yat;
	private BaseTerminal _terminal;

	public ECommandResult Execute(CommandData data)
	{
		var t = (bool)data.Options["-t"];

		_yat = data.Yat;
		_terminal = data.Terminal;

		if (t) CloseTerminal();
		else QuitTheGame();

		return ECommandResult.Success;
	}

	private void CloseTerminal()
	{
		_yat.CallDeferred("CloseTerminal");
	}

	private void QuitTheGame()
	{
		_terminal.Print("Quitting...");
		_yat.GetTree().Quit();
	}
}
