using Confirma.Classes;
using Confirma.Helpers;
using Godot;

namespace Confirma.Scenes;

public partial class TestRunner : Control
{
    [Signal]
    public delegate void TestsExecutionStartedEventHandler();

    [Signal]
    public delegate void TestsExecutionFinishedEventHandler();

#nullable disable
    protected ConfirmaAutoload Autoload { get; set; }
    protected RichTextLabel Output { get; set; }
#nullable restore

    public override void _Ready()
    {
        Output = GetNode<RichTextLabel>("%Output");
        Log.RichOutput = Output;

        // I use GetNodeOrNull instead of the usual GetNode
        // because GetNode prints an error in the editor
        // even though the method execution is successful.
        // This will never be null.
        Autoload = GetNodeOrNull<ConfirmaAutoload>("/root/Confirma");
    }

    public void RunAllTests()
    {
        _ = EmitSignal(SignalName.TestsExecutionStarted);

        Output.Clear();

        TestManager.Props = Autoload.Props;
        TestManager.Run();

        _ = EmitSignal(SignalName.TestsExecutionFinished);
    }
}
