#if TOOLS

using Godot;

namespace Confirma.Scenes;

[Tool]
public partial class ConfirmaBottomPanel : Control
{
#nullable disable
    private Button
        _runAllTests,
        _runCSharpTests,
        _runGdScriptTests,
        _clearOutput,
        _settings;
    private TestRunnerEditor _testRunner;
    private Window _settingsWindow;
    private ConfirmaAutoload _autoload;
#nullable restore

    public override void _Ready()
    {
        _runAllTests = GetNode<Button>("%RunAllTests");
        _runAllTests.Pressed += OnRunAllTestsPressed;

        _runCSharpTests = GetNode<Button>("%RunCSharpTests");
        _runCSharpTests.Pressed += OnRunCSharpTestsPressed;

        _runGdScriptTests = GetNode<Button>("%RunGDScriptTests");
        _runGdScriptTests.Pressed += OnRunGdScriptTestsPressed;

        _clearOutput = GetNode<Button>("%ClearOutput");
        _clearOutput.Pressed += OnClearOutputPressed;

        _testRunner = GetNode<TestRunnerEditor>("%TestRunnerEditor");
        _testRunner.TestsExecutionStarted += OnTestsExecutionStarted;
        _testRunner.TestsExecutionFinished += OnTestsExecutionFinished;

        _settings = GetNode<Button>("%Settings");
        _settings.Pressed += OnSettingsPressed;

        _settingsWindow = GetNode<Window>("%SettingsWindow");

        _ = CallDeferred("LateInit");
    }

    private void LateInit()
    {
        _autoload = GetNode<ConfirmaAutoload>("/root/Confirma");
    }

    private void ResetLanguagesToggle()
    {
        _autoload.Props.DisableCsharp = false;
        _autoload.Props.DisableGdScript = false;
    }

    private void OnRunCSharpTestsPressed()
    {
        ResetLanguagesToggle();

        _autoload.Props.DisableGdScript = true;
        _testRunner.RunAllTests();
    }

    private void OnRunGdScriptTestsPressed()
    {
        ResetLanguagesToggle();

        _autoload.Props.DisableCsharp = true;
        _testRunner.RunAllTests();
    }

    private void OnRunAllTestsPressed()
    {
        ResetLanguagesToggle();
        _testRunner.RunAllTests();
    }

    private void OnClearOutputPressed()
    {
        _testRunner.ClearOutput();
    }

    private void OnSettingsPressed()
    {
        _settingsWindow.Show();
        _settingsWindow.GrabFocus();
    }

    private void OnTestsExecutionStarted()
    {
        _runAllTests.Disabled = true;
        _runCSharpTests.Disabled = true;
        _runGdScriptTests.Disabled = true;
    }

    private void OnTestsExecutionFinished()
    {
        _runAllTests.Disabled = false;
        _runCSharpTests.Disabled = false;
        _runGdScriptTests.Disabled = false;
    }
}

#endif
