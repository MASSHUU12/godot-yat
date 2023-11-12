using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Overlay.Components.Terminal;

[Extension("cubecolor", "Sets the cube's color.", "[b]Usage[/b]: setcube [i]color[/i]")]
public partial class SetCube : IExtension
{
	public static MeshInstance3D Cube { get; set; }

	public CommandResult Execute(ICommand command, params string[] args)
	{
		if (args.Length < 2)
		{
			command.Yat.Terminal.Print("Invalid arguments.", Terminal.PrintType.Error);
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
			command.Yat.Terminal.Print("Invalid color.", Terminal.PrintType.Error);
			return CommandResult.Failure;
		}

		return CommandResult.Success;
	}
}
