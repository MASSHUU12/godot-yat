using System.Reflection;
using Confirma.Classes;
using Godot;

namespace Confirma.Scenes;

public partial class TestRunner : Control
{
#nullable disable
	protected RichTextLabel _output;
	protected TestExecutor _executor;
#nullable restore

	private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

	public override void _Ready()
	{
		_output = GetNode<RichTextLabel>("%Output");
	}

	public void RunAllTests()
	{
		_output.Clear();
		_executor.ExecuteTests(_assembly);
	}
}
