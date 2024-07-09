using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

[Extension("cubecolor", "Sets the cube's color.", "[b]Usage[/b]: setcube [i]color[/i]")]
public sealed class SetCube : IExtension
{
#nullable disable
    public static MeshInstance3D Cube { get; set; }
#nullable restore

    public CommandResult Execute(CommandData data)
    {
        if (data.RawData.Length < 2) return ICommand.InvalidArguments("Invalid arguments.");

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
            return ICommand.Failure("Invalid color.");
        }

        return ICommand.Success();
    }
}
