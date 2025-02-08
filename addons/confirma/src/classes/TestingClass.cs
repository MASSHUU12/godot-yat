using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Confirma.Attributes;
using Confirma.Classes.Discovery;
using Confirma.Enums;
using Confirma.Helpers;
using Confirma.Types;

namespace Confirma.Classes;

public class TestingClass
{
    public Type Type { get; init; }
    public bool IsParallelizable { get; init; }
    public IEnumerable<TestingMethod> TestMethods { get; private set; }

    private readonly string? _setUpName, _tearDownName, _afterAllName, _beforeAllName;

    private TestsProps _props;
    private object? _instance;

    public TestingClass(Type type)
    {
        Type = type;
        TestMethods = CsTestDiscovery.DiscoverTestMethods(type);
        IsParallelizable = type.GetCustomAttribute<ParallelizableAttribute>() is not null;

        _afterAllName = FindLifecycleMethodName<AfterAllAttribute>();
        _beforeAllName = FindLifecycleMethodName<BeforeAllAttribute>();

        _setUpName = FindLifecycleMethodName<SetUpAttribute>();
        _tearDownName = FindLifecycleMethodName<TearDownAttribute>();
    }

    public TestClassResult Run(TestsProps props)
    {
        _props = props;
        List<TestLog> testLogs = [];
        TestResult results = new();
        bool canRunAfterAll = true;

        if (!Type.IsStatic())
        {
            _instance = Activator.CreateInstance(Type);
        }

        if (!InvokeLifecycleMethod(
            _beforeAllName,
            ref testLogs,
            out string beforeAllError
        ))
        {
            AddError(beforeAllError, ref testLogs);
            return new(0, 1, 0, 0, testLogs);
        }

        FilterTestMethodsOnTarget(ref testLogs);

        foreach (TestingMethod method in TestMethods)
        {
            if (!InvokeLifecycleMethod(
                _setUpName,
                ref testLogs,
                out string setUpError
            ))
            {
                results.TestsFailed++;
                canRunAfterAll = false;
                AddError(setUpError, ref testLogs);
                continue;
            }

            TestMethodResult methodResult = method.Run(_props, _instance);
            testLogs.AddRange(methodResult.TestLogs);
            results += methodResult;

            if (!InvokeLifecycleMethod(
                _tearDownName,
                ref testLogs,
                out string tearDownError
            ))
            {
                canRunAfterAll = false;
                results.TestsFailed++;
                AddError(tearDownError, ref testLogs);
            }
        }

        if (canRunAfterAll
            && !InvokeLifecycleMethod(
                _afterAllName,
                ref testLogs,
                out string afterAllError2
            )
        )
        {
            results.TestsFailed++;
            AddError(afterAllError2, ref testLogs);
        }

        return new(
            results.TestsPassed,
            results.TestsFailed,
            results.TestsIgnored,
            results.Warnings,
            testLogs
        );
    }

    private void FilterTestMethodsOnTarget(ref List<TestLog> testLogs)
    {
        if (_props.Target.Target != ERunTargetType.Method
            || string.IsNullOrEmpty(_props.Target.DetailedName)
        )
        {
            return;
        }

        TestMethods = TestMethods
            .Where(tm => tm.Name == _props.Target.DetailedName);

        if (!TestMethods.Any())
        {
            testLogs.Add(new TestLog(
                ELogType.Error,
                ELangType.CSharp,
                $"No test methods found with the name {_props.Target.DetailedName}."
            ));
        }
    }

    private string? FindLifecycleMethodName<T>() where T : Attribute
    {
        Attribute? attribute = Type.GetCustomAttribute<T>();

        return attribute is not LifecycleAttribute la
            ? null
            : la.MethodName;
    }

    private bool InvokeLifecycleMethod(
        string? methodName,
        ref List<TestLog> testLogs,
        out string error
    )
    {
        error = string.Empty;
        if (string.IsNullOrEmpty(methodName)) { return true; }

        if (_props.IsVerbose)
        {
            testLogs.Add(new TestLog(ELogType.Info, $"[{methodName}] {Type.Name}"));
        }

        if (Type.GetMethod(methodName) is not MethodInfo method)
        {
            error = $"Lifecycle method {methodName} not found.";
            return false;
        }

        try
        {
            _ = method.Invoke(_instance, null);
        }
        catch (Exception e)
        {
            error = e.InnerException?.Message
                    ?? e.Message
                    ?? $"Calling lifecycle method {methodName} failed.";
            error = $"Error in lifecycle method {methodName}: {error}";
            return false;
        }

        return true;
    }

    private static void AddError(string error, ref List<TestLog> testLogs)
    {
        testLogs.Add(new(ELogType.Error, $"- {error}\n"));
    }
}
