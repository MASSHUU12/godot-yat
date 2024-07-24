#if TOOLS

using Godot;

namespace Confirma.Scenes;

[Tool]
public partial class TestBottomPanel : Control
{
#nullable disable
    private Button _runAllTests;
    private Button _clearOutput;
    private CheckBox _verbose;
    private TestRunnerEditor _testRunner;
    private ConfirmaAutoload _autoload;
#nullable restore

    public override void _Ready()
    {
        _runAllTests = GetNode<Button>("%RunAllTests");
        _runAllTests.Pressed += OnRunAllTestsPressed;

        _clearOutput = GetNode<Button>("%ClearOutput");
        _clearOutput.Pressed += OnClearOutputPressed;

        _verbose = GetNode<CheckBox>("%Verbose");

        _testRunner = GetNode<TestRunnerEditor>("%TestRunnerEditor");

        _ = CallDeferred("LateInit");
    }

    private void LateInit()
    {
        _autoload = GetNode<ConfirmaAutoload>("/root/Confirma");

        _verbose.Toggled += (bool on) => _autoload.Props.IsVerbose = on;
    }

    private void OnRunAllTestsPressed()
    {
        _testRunner.RunAllTests();
    }

    private void OnClearOutputPressed()
    {
        _testRunner.ClearOutput();
    }
}

#endif
