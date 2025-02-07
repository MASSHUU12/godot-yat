using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;
using static Godot.DisplayServer;

namespace YAT.Commands;

[Command("tfs", "Toggles the full screen mode.")]
[Option("-e", "bool", "Enables the exclusive full screen mode.")]
public sealed class Tfs : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        bool exclusive = (bool)data.Options["-e"];

        if (WindowGetMode() == WindowMode.Windowed)
        {
            WindowSetMode(
                exclusive
                ? WindowMode.ExclusiveFullscreen
                : WindowMode.Fullscreen
            );
        }
        else
        {
            WindowSetMode(WindowMode.Windowed);
        }

        return ICommand.Success(
            [WindowGetMode().ToString()],
            $"Toggled screen mode to {WindowGetMode()}."
        );
    }
}
