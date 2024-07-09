namespace Confirma.Classes;

public class TestMethodResult
{
    public uint TestsPassed { get; set; }
    public uint TestsFailed { get; set; }
    public uint TestsIgnored { get; set; }
    public uint Warnings { get; set; }

    public TestMethodResult(uint passed, uint failed, uint ignored, uint warnings)
    {
        TestsPassed = passed;
        TestsFailed = failed;
        TestsIgnored = ignored;
        Warnings = warnings;
    }

    public TestMethodResult()
    {
        TestsPassed = 0;
        TestsFailed = 0;
        TestsIgnored = 0;
        Warnings = 0;
    }

    public void Reset()
    {
        TestsPassed = 0;
        TestsFailed = 0;
        TestsIgnored = 0;
        Warnings = 0;
    }
}
