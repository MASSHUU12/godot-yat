using System.Reflection;
using Confirma.Classes;
using Confirma.Helpers;
using Godot;

namespace Confirma.Scenes;

public partial class TestRunner : Control
{
#nullable disable
    protected ConfirmaAutoload _autoload;
    protected RichTextLabel _output;
#nullable restore

    private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    public override void _Ready()
    {
        _output = GetNode<RichTextLabel>("%Output");
        Log.RichOutput = _output;

        _autoload = GetNode<ConfirmaAutoload>("/root/Confirma");
    }

    public void RunAllTests(string className = "")
    {
        _output.Clear();

        TestExecutor.Props = _autoload.Props;
        TestExecutor.ExecuteTests(_assembly, className);
    }
}
