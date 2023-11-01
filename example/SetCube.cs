using Godot;
using YAT.Commands;

[Extension("cubecolor", "Sets the cube's color.", "[b]Usage[/b]: setcube [i]color[/i]")]
public partial class SetCube : IExtension
{
	public static MeshInstance3D Cube { get; set; }

	public CommandResult Execute(YAT.YAT yat, ICommand command, params string[] args)
	{
		if (args.Length < 2)
		{
			yat.Terminal.Println("Invalid arguments.", YAT.Terminal.PrintType.Error);
			return CommandResult.InvalidArguments;
		}

		try
		{
			Color color = new(args[1]);
			Cube.MaterialOverride = new StandardMaterial3D
			{
				AlbedoColor = color
			};
		}
		catch (System.Exception)
		{
			yat.Terminal.Println("Invalid color.", YAT.Terminal.PrintType.Error);
			return CommandResult.Failure;
		}

		return CommandResult.Success;
	}
}
