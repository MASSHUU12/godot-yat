using System.Linq;
using Godot;

namespace Confirma.Scenes;

public partial class ConfirmaAutoload : Node
{
	public bool IsHeadless { get; private set; } = false;
	public bool QuitAfterTests { get; private set; } = false;

	private const string _testRunnerUID = "uid://cq76c14wl2ti3";
	private const string _paramToRunTests = "--confirma-run";
	private const string _paramQuitAfterTests = "--confirma-quit";

	public override void _Ready()
	{
		if (!OS.GetCmdlineUserArgs().Contains(_paramToRunTests)) return;
		if (OS.GetCmdlineUserArgs().Contains(_paramQuitAfterTests)) QuitAfterTests = true;
		if (DisplayServer.GetName() == "headless") IsHeadless = true;

		RunTests();
	}

	private void RunTests()
	{
		GetTree().CallDeferred("change_scene_to_file", _testRunnerUID);

		if (QuitAfterTests) GetTree().Quit();
	}
}
