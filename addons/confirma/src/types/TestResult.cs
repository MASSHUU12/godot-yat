namespace Confirma.Types;

public record TestResult
{
    public uint TotalTests { get; set; } = 0;
    public uint TestsPassed { get; set; } = 0;
    public uint TestsFailed { get; set; } = 0;
    public uint TestsIgnored { get; set; } = 0;
    public double TotalTime { get; set; } = 0;
    public uint Warnings { get; set; } = 0;

    public TestResult() { }

    public static TestResult operator +(TestResult a, TestClassResult b)
    {
        a.TotalTests += b.TestsPassed + b.TestsFailed + b.TestsIgnored;
        a.TestsPassed += b.TestsPassed;
        a.TestsFailed += b.TestsFailed;
        a.TestsIgnored += b.TestsIgnored;
        a.Warnings += b.Warnings;

        return a;
    }

    public static TestResult operator +(TestResult a, TestResult b)
    {
        a.TotalTests += b.TestsPassed + b.TestsFailed + b.TestsIgnored;
        a.TestsPassed += b.TestsPassed;
        a.TestsFailed += b.TestsFailed;
        a.TestsIgnored += b.TestsIgnored;
        a.Warnings += b.Warnings;

        return a;
    }
}
