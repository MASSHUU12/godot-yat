using System.Reflection;
using Confirma.Classes;
using Confirma.Helpers;
using Godot;

namespace Confirma.Scenes;

public partial class TestRunner : Control
{
#nullable disable
    protected ConfirmaAutoload Autoload;
    protected RichTextLabel Output;
#nullable restore

    private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    public override void _Ready()
    {
        Output = GetNode<RichTextLabel>("%Output");
        Log.RichOutput = Output;

        Autoload = GetNode<ConfirmaAutoload>("/root/Confirma");
    }

    public void RunAllTests(string className = "")
    {
        Output.Clear();

        TestExecutor.Props = Autoload.Props;
        TestExecutor.ExecuteTests(_assembly, className);
    }
}
