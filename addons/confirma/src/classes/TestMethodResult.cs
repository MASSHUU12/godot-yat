using System.Collections.Generic;

namespace Confirma.Classes;

public class TestMethodResult
{
    public uint TestsPassed { get; set; }
    public uint TestsFailed { get; set; }
    public uint TestsIgnored { get; set; }
    public uint Warnings { get; set; }
    public List<TestLog> TestLogs { get; set; }

    public TestMethodResult(
        uint passed,
        uint failed,
        uint ignored,
        uint warnings,
        List<TestLog> logs
    )
    {
        TestsPassed = passed;
        TestsFailed = failed;
        TestsIgnored = ignored;
        Warnings = warnings;
        TestLogs = logs;
    }

    public TestMethodResult()
    {
        TestsPassed = 0;
        TestsFailed = 0;
        TestsIgnored = 0;
        Warnings = 0;
        TestLogs = new();
    }

    public void Reset()
    {
        TestsPassed = 0;
        TestsFailed = 0;
        TestsIgnored = 0;
        Warnings = 0;
        TestLogs = new();
    }
}
