using System;

namespace Confirma.Types;

public struct TestsProps
{
    public event Action? ExitOnFailure;

    public TestResult Result { get; set; } = new();

    public bool RunTests { get; set; }
    public bool IsVerbose { get; set; }
    public bool IsHeadless { get; set; }
    public bool ExitOnFail { get; set; }
    public bool QuitAfterTests { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string MethodName { get; set; } = string.Empty;
    public bool DisableParallelization { get; set; }

    public TestsProps() { }

    public void ResetStats()
    {
        Result = new TestResult();
    }

    public readonly void CallExitOnFailure()
    {
        ExitOnFailure?.Invoke();
    }
}
