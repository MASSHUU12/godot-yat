#if TOOLS

using Godot;

namespace Confirma.Scenes;

[Tool]
public partial class TestBottomPanel : Control
{
#nullable disable
	private Button _runAllTests;
	private Button _clearOutput;
	private TestRunnerEditor _testRunner;
#nullable restore

	public override void _Ready()
	{
		_runAllTests = GetNode<Button>("%RunAllTests");
		_runAllTests.Pressed += OnRunAllTestsPressed;

		_clearOutput = GetNode<Button>("%ClearOutput");
		_clearOutput.Pressed += OnClearOutputPressed;

		_testRunner = GetNode<TestRunnerEditor>("%TestRunnerEditor");
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
