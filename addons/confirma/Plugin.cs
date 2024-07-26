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
        _testBottomPanel = GD.Load<PackedScene>(
            GetPluginPath() + "/src/scenes/test_bottom_panel/TestBottomPanel.tscn"
        ).Instantiate<Control>();
        _ = AddControlToBottomPanel(_testBottomPanel, "Confirma");

        AddAutoloadSingleton("Confirma", GetPluginPath() + "/src/scenes/confirma_autoload/ConfirmaAutoload.tscn");

        GD.Print("Confirma is ready!");
    }

    public override void _ExitTree()
    {
        RemoveAutoloadSingleton("Confirma");
        RemoveControlFromBottomPanel(_testBottomPanel);

        _testBottomPanel.Free();

        GD.Print("Confirma is disabled!");
    }

    private string GetPluginPath()
    {
        return GetScript().As<Script>().ResourcePath.GetBaseDir();
    }
}
#endif
