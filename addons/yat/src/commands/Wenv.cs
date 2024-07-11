using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("wenv", "Manages the world environment.")]
[Argument("action", "remove|restore", "Removes or restores the world environment.")]
public sealed class Wenv : ICommand
{
#nullable disable
    private static Environment _world3DEnvironment;
#nullable restore

    public CommandResult Execute(CommandData data)
    {
        var action = (string)data.Arguments["action"];
        World3D world = data.Yat.GetTree().Root.World3D;

        return action == "remove"
            ? RemoveEnvironment(world)
            : RestoreEnvironment(world);
    }

    private static CommandResult RestoreEnvironment(World3D world)
    {
        if (world is null)
        {
            return ICommand.Failure("No world to restore environment to.");
        }

        if (_world3DEnvironment is null)
        {
            return ICommand.Failure("No environment to restore.");
        }

        world.Environment = _world3DEnvironment;
        _world3DEnvironment = null;

        return ICommand.Success("Restored environment.");
    }

    private static CommandResult RemoveEnvironment(World3D world)
    {
        Environment? env = world?.Environment;

        if (world is null)
        {
            return ICommand.Failure("No world to remove environment from.");
        }

        if (env is null)
        {
            return ICommand.Failure("No environment to remove.");
        }

        _world3DEnvironment = env;
        world.Environment = null;

        return ICommand.Success("Removed environment.");
    }
}
