using Godot;
using YAT.Commands;

public partial class Main : Node3D
{
	public override void _Ready()
	{
		var cube = GetNode<MeshInstance3D>("Scene/Cube");
		var set = GetNode<YAT.YAT>("/root/YAT").Commands["set"] as Set;

		SetCube.Cube = cube;
		set.Extension.Register(new SetCube());
	}
}
