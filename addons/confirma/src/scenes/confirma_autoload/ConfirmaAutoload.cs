using System;
using Confirma.Classes;
using Confirma.Helpers;
using Confirma.Types;
using Godot;

namespace Confirma.Scenes;

[Tool]
public partial class ConfirmaAutoload : Node
{
    public TestsProps Props = new();

    public override void _Ready()
    {
        CheckArguments();

        if (!Props.RunTests)
        {
            return;
        }

        SetupGlobals();
        ChangeScene();
    }

    private void SetupGlobals()
    {
        Log.IsHeadless = Props.IsHeadless;
        Global.Root = GetTree().Root;
    }

    private void CheckArguments()
    {
        string[] args = OS.GetCmdlineUserArgs();

        if (DisplayServer.GetName() == "headless")
        {
            Props.IsHeadless = true;
        }

        foreach (string arg in args)
        {
            if (!Props.RunTests && arg.StartsWith("--confirma-run", StringComparison.InvariantCulture))
            {
                Props.RunTests = true;

                Props.ClassName = arg.Find('=') == -1
                    ? string.Empty
                    : arg.Split('=')[1];

                continue;
            }
            else if (Props.RunTests
                && !Props.ClassName.Equals(string.Empty, StringComparison.Ordinal)
                && arg.StartsWith("--confirma-method", StringComparison.InvariantCulture)
            )
            {
                Props.MethodName = arg.Find('=') == -1
                                    ? string.Empty
                                    : arg.Split('=')[1];

                continue;
            }

            if (!Props.QuitAfterTests && arg == "--confirma-quit")
            {
                Props.QuitAfterTests = true;
                continue;
            }

            if (!Props.ExitOnFail && arg == "--confirma-exit-on-failure")
            {
                Props.ExitOnFail = true;
                continue;
            }

            if (!Props.IsVerbose && arg == "--confirma-verbose")
            {
                Props.IsVerbose = true;
                continue;
            }

            if (!Props.DisableParallelization && arg == "--confirma-sequential")
            {
                Props.DisableParallelization = true;
            }
        }
    }

    private void ChangeScene()
    {
        _ = GetTree().CallDeferred("change_scene_to_file", "uid://cq76c14wl2ti3");

        if (Props.QuitAfterTests)
        {
            GetTree().Quit();
        }
    }
}
