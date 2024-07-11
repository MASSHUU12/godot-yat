using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("version", "Displays the current game version.")]
public sealed class Version : ICommand
{
    private static readonly string _gameName, _gameVersion, _version;

    static Version()
    {
        _gameName = ProjectSettings.GetSetting("application/config/name").ToString();
        _gameVersion = ProjectSettings.GetSetting("application/config/version").ToString();

        if (string.IsNullOrEmpty(_gameName))
        {
            _gameName = "Your Awesome Game";
        }

        if (string.IsNullOrEmpty(_gameVersion))
        {
            _gameVersion = "v0.0.0";
        }

        _version = $"{_gameName} {_gameVersion}";
    }

    public CommandResult Execute(CommandData data)
    {
        return ICommand.Ok(_version);
    }
}
