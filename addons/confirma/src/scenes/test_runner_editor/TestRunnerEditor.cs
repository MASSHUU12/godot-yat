#if TOOLS

using System;
using Confirma.Helpers;
using Godot;

namespace Confirma.Scenes;

[Tool]
public partial class TestRunnerEditor : TestRunner
{
	public override void _Ready()
	{
		base._Ready();

		_executor = new(new Log(_output), new(false));

		ClearOutput();
	}

	public void ClearOutput()
	{
		_output.Clear();
	}
}

#endif
