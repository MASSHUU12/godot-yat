using System.Collections.Generic;
using Confirma.Classes;

namespace Confirma.Types;

public record TestResult
{
    public uint TotalTests { get; set; }
    public uint TestsPassed { get; set; }
    public uint TestsFailed { get; set; }
    public uint TestsIgnored { get; set; }
    public uint TotalOrphans { get; set; }
    public uint TotalClasses { get; set; }
    public double TotalTime { get; set; }
    public uint Warnings { get; set; }
    public List<TestLog> TestLogs { get; set; } = [];

    public TestResult() { }

    public static TestResult operator +(TestResult a, TestClassResult b)
    {
        a.TotalTests += b.TestsPassed + b.TestsFailed + b.TestsIgnored;
        a.TestsPassed += b.TestsPassed;
        a.TestsFailed += b.TestsFailed;
        a.TestsIgnored += b.TestsIgnored;
        a.Warnings += b.Warnings;
        a.TestLogs.AddRange(b.TestLogs);

        return a;
    }

    public static TestResult operator +(TestResult a, TestMethodResult b)
    {
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
        a.TotalOrphans += b.TotalOrphans;
        a.TotalClasses += b.TotalClasses;
        a.TestsIgnored += b.TestsIgnored;
        a.Warnings += b.Warnings;
        a.TestLogs.AddRange(b.TestLogs);

        return a;
    }
}
