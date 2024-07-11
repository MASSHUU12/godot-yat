using System.Linq;
using Godot;

namespace YAT.Scenes;

public partial class YatEnable : Node
{
    [Export(PropertyHint.Flags, "UserData,CurrentDirectory,CmdArgument")]
    public short YatEnableAction { get; set; } = 0;

    [Export] public string FileName { get; set; } = ".yatenable";
    [Export] public string ArgumentName { get; set; } = "--yat";

    public bool YatEnabled { get; set; } = true;

    public override void _Ready()
    {
        if (YatEnableAction == 0)
        {
            return;
        }

        if ((YatEnableAction & 0b0001) == 1)
        {
            YatEnabled = FileAccess.FileExists("user://" + FileName);

            if (YatEnabled)
            {
                return;
            }
        }

        if ((YatEnableAction & 0b0010) == 2)
        {
            YatEnabled = FileAccess.FileExists("res://" + FileName);

            if (YatEnabled)
            {
                return;
            }
        }

        if ((YatEnableAction & 0b0100) == 4)
        {
            YatEnabled = OS.GetCmdlineUserArgs().Contains(ArgumentName);

            if (YatEnabled)
            {
                return;
            }
        }

        YatEnabled = false;
    }
}
