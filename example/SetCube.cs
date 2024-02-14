using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Types;

[Extension("cubecolor", "Sets the cube's color.", "[b]Usage[/b]: setcube [i]color[/i]")]
public sealed class SetCube : IExtension
{
	public static MeshInstance3D Cube { get; set; }

	public ECommandResult Execute(CommandData data)
	{
		if (data.RawData.Length < 2)
		{
			data.Terminal.Print("Invalid arguments.", EPrintType.Error);
			return ECommandResult.InvalidArguments;
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
			data.Terminal.Print("Invalid color.", EPrintType.Error);
			return ECommandResult.Failure;
		}

		return ECommandResult.Success;
	}
}
