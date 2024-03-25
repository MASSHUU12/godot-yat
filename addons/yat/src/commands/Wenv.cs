using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("wenv", "Manages the world environment.", "[b]Usage[/b]: wenv [i]action[/i]")]
[Argument("action", "remove|restore", "Removes or restores the world environment.")]
public sealed class Wenv : ICommand
{
#nullable disable
	private static Environment _world3DEnvironment;
#nullable restore

	public CommandResult Execute(CommandData data)
	{
		var action = (string)data.Arguments["action"];
		var world = data.Yat.GetTree().Root.World3D;

		if (action == "remove") return RemoveEnvironment(world);
		return RestoreEnvironment(world);
	}

	private static CommandResult RestoreEnvironment(World3D world)
	{
		if (world is null) return ICommand.Failure("No world to restore environment to.");
		if (_world3DEnvironment is null) return ICommand.Failure("No environment to restore.");

		world.Environment = _world3DEnvironment;
		_world3DEnvironment = null;

		return ICommand.Success("Restored environment.");
	}

	private static CommandResult RemoveEnvironment(World3D world)
	{
		var env = world?.Environment;

		if (world is null) return ICommand.Failure("No world to remove environment from.");
		if (env is null) return ICommand.Failure("No environment to remove.");

		_world3DEnvironment = env;
		world.Environment = null;

		return ICommand.Success("Removed environment.");
	}
}
