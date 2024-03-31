using Godot;

namespace YAT.Scenes;

public partial class YatEnable : Node
{
	[Export(PropertyHint.Flags, "UserData,CurrentDirectory,CmdArgument")]
	public short YatEnableAction { get; set; } = 0;

	public override void _Ready()
	{
	}
}
