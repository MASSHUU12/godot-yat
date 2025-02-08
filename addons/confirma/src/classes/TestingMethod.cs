using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Confirma.Attributes;
using Confirma.Classes.Discovery;
using Confirma.Enums;
using Confirma.Exceptions;
using Confirma.Fuzz;
using Confirma.Helpers;
using Confirma.Types;

using static Confirma.Enums.ETestCaseState;

namespace Confirma.Classes;

public class TestingMethod
{
    public MethodInfo Method { get; }
    public IEnumerable<TestCase> TestCases { get; }
    public string Name { get; }
    public TestMethodResult Result { get; }

    public TestingMethod(MethodInfo method)
    {
        Result = new();
        Method = method;
        TestCases = DiscoverTestCases();
        Name = Method.GetCustomAttribute<TestNameAttribute>()?.Name ?? Method.Name;
    }

    public TestMethodResult Run(TestsProps props, object? instance)
    {
        foreach (TestCase test in TestCases)
        {
            int iterations = CalculateIterations(test);

            for (int i = 0; i <= iterations; i++)
            {
                if (CheckIfIgnored(test, props))
                {
                    continue;
                }

                ExecuteTestWithRepeats(test, props, instance);
            }
        }

        return Result;
    }

    private static int CalculateIterations(TestCase test)
    {
        return test.Repeat?.IsFlaky == true ? 0 : test.Repeat?.Repeat ?? 0;
    }

    private bool CheckIfIgnored(TestCase test, TestsProps props)
    {
        IgnoreAttribute? attr = test.Method.GetCustomAttribute<IgnoreAttribute>();
        if ((attr?.IsIgnored(props.Target)) != true)
        {
            return false;
        }

        if (attr.HideFromResults == true)
        {
            return true;
        }

        Result.TestsIgnored++;
        TestLog log = new(
            ELogType.Method,
            Name,
            Ignored,
            0,
            test.Params,
            attr.Reason
        );
        Result.TestLogs.Add(log);
        return true;
    }

    private void ExecuteTestWithRepeats(
        TestCase test,
        TestsProps props,
        object? instance
    )
    {
        int repeats = 0;
        int maxRepeats = test.Repeat?.GetFlakyRetries ?? 0;
        int backoff = test.Repeat?.Backoff ?? 0;
        Stopwatch sw = Stopwatch.StartNew();

        do
        {
            try
            {
                test.Run(instance);
                sw.Stop();
                LogTestResult(Passed, test, null, sw.ElapsedMilliseconds);
                Result.TestsPassed++;
                break;
            }
            catch (ConfirmAssertException e)
            {
                if (ShouldRetryFlakyTest(ref repeats, maxRepeats, backoff))
                {
                    continue;
                }

                LogTestResult(Failed, test, e.Message, sw.ElapsedMilliseconds);
                Result.TestsFailed++;

                if (test.Repeat?.FailFast == true)
                {
                    break;
                }

                if (props.ExitOnFail)
                {
                    props.CallExitOnFailure();
                }
            }
        } while (repeats < maxRepeats);
    }

    private static bool ShouldRetryFlakyTest(
        ref int repeats,
        int maxRepeats,
        int backoff
    )
    {
        if (maxRepeats == 0 || repeats >= maxRepeats)
        {
            return false;
        }

        repeats++;
        if (backoff > 0)
        {
            // Workaround for:
            // https://github.com/godotengine/godot/issues/94510
            Task task = Task.Run(
                async () => await Task.Delay(backoff)
            );
            task.Wait();
        }
        return true;
    }

    private void LogTestResult(
        ETestCaseState state,
        TestCase test,
        string? message,
        long executionTime
    )
    {
        Result.TestLogs.Add(
            new(
                ELogType.Method,
                Name,
                state,
                executionTime,
                test.Params,
                message,
                ELangType.CSharp
            )
        );
    }

    private List<TestCase> DiscoverTestCases()
    {
        using IEnumerator<System.Attribute> discovered = CsTestDiscovery
            .GetAttributesForTestCaseGeneration(Method)
            .GetEnumerator();

        List<TestCase> cases = [];
        List<FuzzGenerator> generators = [];
        RepeatAttribute? pendingRepeat = null;
        RepeatAttribute? fuzzRepeat = null;

        while (discovered.MoveNext())
        {
            switch (discovered.Current)
            {
                case RepeatAttribute repeat:
                    if (pendingRepeat is not null)
                    {
                        Log.PrintWarning(
                            $"The Repeat attributes for the {Method.Name} "
                            + "cannot occur in succession.\n"
                        );
                        Result.Warnings++;
                        break;
                    }

                    pendingRepeat = repeat;
                    continue;

                case TestCaseAttribute testCase:
                    cases.Add(new(Method, testCase.Parameters, pendingRepeat));
                    pendingRepeat = null;
                    continue;

                case FuzzAttribute fuzz:
                    generators.Add(fuzz.Generator);

                    if (fuzzRepeat is null)
                    {
                        fuzzRepeat = pendingRepeat;
                        pendingRepeat = null;
                    }
                    else if (pendingRepeat is not null)
                    {
                        Log.PrintWarning(
                            "Multiple Repeat attributes were detected associated"
                            + " with the Fuzz attributes for method "
                            + $"{Method.Name}. Only the first one will be used.\n"
                        );
                        pendingRepeat = null;
                        Result.Warnings++;
                    }
                    continue;

                default:
                    // Unexpected attributes
                    continue;
            }
        }

        ResolveUnusedRepeat(pendingRepeat);
        HandleFuzzGenerators(generators, fuzzRepeat, cases);

        return cases;
    }

    private void ResolveUnusedRepeat(RepeatAttribute? pendingRepeat)
    {
        if (pendingRepeat is not null)
        {
            Log.PrintWarning(
                $"The Repeat attribute for {Method.Name} " +
                "will be ignored because it does not have a " +
                "TestCase/Fuzz attribute associated with it.\n"
            );
            Result.Warnings++;
        }
    }

    private void HandleFuzzGenerators(
        List<FuzzGenerator> generators,
        RepeatAttribute? fuzzRepeat,
        List<TestCase> cases
    )
    {
        if (generators.Count == 0)
        {
            return;
        }

        int methodParams = Method.GetParameters().Length;

        if (generators.Count > methodParams)
        {
            Log.PrintWarning(
                $"Detected {generators.Count} Fuzz attributes but "
                + $"{Method.Name} contains only {methodParams} parameters. "
                + "Excessive Fuzz attributes are ignored.\n"
            );
            Result.Warnings++;
        }

        cases.Add(new(
            Method,
            [.. generators
                .Take(methodParams) // Ignore excessive attributes
                .Select(static gen => gen.NextValue())
            ],
            fuzzRepeat
        ));
    }
}
