#if TOOLS

using Godot;

namespace Confirma.Scenes;

[Tool]
public partial class TestRunnerEditor : TestRunner
{
    public override void _Ready()
    {
        CallDeferred("LateInit");
    }

    private void LateInit()
    {
        base._Ready();

        ClearOutput();
    }

    public void ClearOutput()
    {
        _output.Clear();
    }
}

#endif
