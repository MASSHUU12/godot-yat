using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

[Extension("cubecolor", "Sets the cube's color.", "[b]Usage[/b]: setcube [i]color[/i]")]
public partial class SetCube : IExtension
{
	public static MeshInstance3D Cube { get; set; }

	public CommandResult Execute(CommandData data)
	{
		if (data.RawData.Length < 2)
		{
			data.Terminal.Print("Invalid arguments.", PrintType.Error);
			return CommandResult.InvalidArguments;
		}

		try
		{
			Color color = new(data.RawData[1]);
			Cube.MaterialOverride = new StandardMaterial3D
			{
				AlbedoColor = color
			};
		}
		catch (System.Exception)
		{
			data.Terminal.Print("Invalid color.", PrintType.Error);
			return CommandResult.Failure;
		}

		return CommandResult.Success;
	}
}
