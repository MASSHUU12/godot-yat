using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("fov", "Sets the field of view for the camera.", "[b]Usage[/b]: fov [i]fov[/i]")]
[Argument("fov", "float(1:179)", "The field of view to set.")]
public sealed class Fov : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var fov = (float)data.Arguments["fov"];
		var camera = data.Yat.GetTree().Root.GetCamera3D();

		if (camera is null) return ICommand.Failure("No 3D camera found.");

		camera.Fov = fov;

		return ICommand.Success($"Field of view set to {fov}.");
	}
}
