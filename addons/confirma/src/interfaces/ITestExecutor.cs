using Confirma.Types;

namespace Confirma.Interfaces;

public interface ITestExecutor
{
    // public TestsProps Props { get; set; }

    public int Execute(out TestResult? result);
}
