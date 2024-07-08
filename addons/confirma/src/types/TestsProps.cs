using System;

namespace Confirma.Types;

public struct TestsProps
{
    public event Action? ExitOnFailure;

    public TestResult Result { get; set; } = new();

    public bool RunTests { get; set; } = false;
    public bool IsVerbose { get; set; } = false;
    public bool IsHeadless { get; set; } = false;
    public bool ExitOnFail { get; set; } = false;
    public bool QuitAfterTests { get; set; } = false;
    public string ClassName { get; set; } = string.Empty;
    public bool DisableParallelization { get; set; } = false;

    public TestsProps() { }

    public void ResetStats()
    {
        Result = new();
    }

    public readonly void CallExitOnFailure()
    {
        ExitOnFailure?.Invoke();
    }
}
