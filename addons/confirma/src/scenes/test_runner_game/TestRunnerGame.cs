using Confirma.Helpers;

namespace Confirma.Scenes;

public partial class TestRunnerGame : TestRunner
{
#nullable disable
	private ConfirmaAutoload _confirmaAutoload;
#nullable restore

	public override void _Ready()
	{
		base._Ready();

		_confirmaAutoload = GetNode<ConfirmaAutoload>("/root/Confirma");

		_executor = new(
			new Log(_confirmaAutoload.IsHeadless ? null : _output),
			new(_confirmaAutoload.IsHeadless)
		);

		RunIfRoot();
	}

	private void RunIfRoot()
	{
		if (GetTree().CurrentScene != this) return;

		RunAllTests();
	}
}
