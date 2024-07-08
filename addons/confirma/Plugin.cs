#if TOOLS
using Godot;

namespace Confirma;

[Tool]
public partial class Plugin : EditorPlugin
{
#nullable disable
    private Control _testBottomPanel;
#nullable restore

    public override void _EnterTree()
    {
        _testBottomPanel = GD.Load<PackedScene>("uid://bl21wviqvff84").Instantiate<Control>();

        AddAutoloadSingleton("Confirma", "res://addons/confirma/src/scenes/confirma_autoload/ConfirmaAutoload.tscn");
        AddControlToBottomPanel(_testBottomPanel, "Confirma");

        GD.Print("Confirma is ready!");
    }

    public override void _ExitTree()
    {
        RemoveAutoloadSingleton("Confirma");
        RemoveControlFromBottomPanel(_testBottomPanel);

        GD.Print("Confirma is disabled!");
    }
}
#endif
