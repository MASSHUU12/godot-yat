using System;
using Confirma.Enums;
using Confirma.Scenes;
using Godot;

namespace Confirma.Types;

public struct TestsProps()
{
    public event Action? ExitOnFailure;

    public RunTarget Target { get; set; }
    public TestResult Result { get; set; } = new();
    public ConfirmaAutoload? Autoload { get; set; }

    public bool RunTests { get; set; }
    public bool ShowHelp { get; set; }
    public bool IsVerbose { get; set; }
    public bool IsHeadless { get; set; }
    public bool ExitOnFail { get; set; }
    public bool DisableCsharp { get; set; }
    public bool MonitorOrphans { get; set; } = true;
    public bool DisableGdScript { get; set; }
    public bool DisableParallelization { get; set; }
    public string GdTestPath { get; set; } = ProjectSettings
        .GetSetting(
            "confirma/config/gdscript_tests_folder",
            // Note: When changing path here,
            // remember to change it also in Plugin.cs.
            "res://gdtests/"
        )
        .AsString();
    public string OutputPath { get; set; } = ProjectSettings
        .GetSetting(
            "confirma/config/output_path",
            // Note: When changing path here,
            // remember to change it also in Plugin.cs.
            "./test_results.json"
        )
        .AsString();
    public string SelectedHelpPage { get; set; } = "default";

    public ELogOutputType OutputType { get; set; } = ELogOutputType.Log;

    public void ResetStats()
    {
        Result = new TestResult();
    }

    public readonly void CallExitOnFailure()
    {
        ExitOnFailure?.Invoke();
    }
}
