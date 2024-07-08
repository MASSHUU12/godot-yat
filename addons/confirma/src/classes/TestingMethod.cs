using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Confirma.Attributes;
using Confirma.Exceptions;
using Confirma.Helpers;
using Confirma.Types;

using static Confirma.Enums.ETestCaseState;

namespace Confirma.Classes;

public class TestingMethod
{
    public MethodInfo Method { get; }
    public IEnumerable<TestCase> TestCases { get; }
    public string Name { get; }
    public TestMethodResult Result { get; private set; }

    public TestingMethod(MethodInfo method)
    {
        Result = new();
        Method = method;
        TestCases = DiscoverTestCases();
        Name = Method.GetCustomAttribute<TestNameAttribute>()?.Name ?? Method.Name;
    }

    public TestMethodResult Run(TestsProps props)
    {
        foreach (TestCase test in TestCases)
        {
            for (ushort i = 0; i <= test.Repeat; i++)
            {
                var attr = test.Method.GetCustomAttribute<IgnoreAttribute>();
                if (attr is not null && attr.IsIgnored())
                {
                    Result.TestsIgnored++;

                    TestOutput.PrintOutput(Name, test.Params, Ignored, props.IsVerbose, attr.Reason);
                    continue;
                }

                try
                {
                    test.Run();
                    Result.TestsPassed++;

                    TestOutput.PrintOutput(Name, test.Params, Passed, props.IsVerbose);
                }
                catch (ConfirmAssertException e)
                {
                    Result.TestsFailed++;

                    TestOutput.PrintOutput(Name, test.Params, Failed, props.IsVerbose, e.Message);

                    if (props.ExitOnFail) props.CallExitOnFailure();
                }
            }
        }

        return Result;
    }

    private IEnumerable<TestCase> DiscoverTestCases()
    {
        List<TestCase> cases = new();
        var discovered = TestDiscovery.GetTestCasesFromMethod(Method).GetEnumerator();

        while (discovered.MoveNext())
        {
            if (discovered.Current is TestCaseAttribute testCase)
            {
                cases.Add(new(Method, testCase.Parameters, 0));
                continue;
            }

            // I rely on the order in which the attributes are defined
            // to determine which TestCase attributes should be assigned values
            // from the Repeat attributes.
            if (discovered.Current is RepeatAttribute repeat)
            {
                if (!discovered.MoveNext())
                {
                    Log.PrintWarning(
                        $"The Repeat attribute for the \"{Method.Name}\" method will be ignored " +
                        "because it does not have the TestCase attribute after it.\n"
                    );
                    Result.Warnings++;
                    continue;
                }

                if (discovered.Current is RepeatAttribute)
                {
                    Log.PrintWarning(
                        $"The Repeat attributes for the \"{Method.Name}\" cannot occur in succession.\n"
                    );
                    Result.Warnings++;
                    continue;
                }

                if (discovered.Current is not TestCaseAttribute tc) continue;

                cases.Add(new(Method, tc.Parameters, repeat.Repeat));
            }
        }

        return cases.AsEnumerable();
    }
}
