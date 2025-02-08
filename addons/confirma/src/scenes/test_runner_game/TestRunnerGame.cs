namespace Confirma.Scenes;

public partial class TestRunnerGame : TestRunner
{
    public override void _Ready()
    {
        base._Ready();

        RunIfRoot();
    }

    private void RunIfRoot()
    {
        if (GetTree().CurrentScene != this)
        {
            return;
        }

        RunAllTests();
    }
}
