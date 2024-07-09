using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Confirma.Attributes;
using Confirma.Helpers;
using Confirma.Types;

namespace Confirma.Classes;

public class TestingClass
{
    public readonly Type Type;
    public readonly bool IsParallelizable;
    public readonly IEnumerable<TestingMethod> TestMethods;

    private TestsProps _props;
    private readonly Dictionary<string, LifecycleMethodData> _lifecycleMethods = new();

    public TestingClass(Type type)
    {
        Type = type;
        TestMethods = TestDiscovery.DiscoverTestMethods(type);
        IsParallelizable = type.GetCustomAttribute<ParallelizableAttribute>() is not null;

        InitialLookup();
    }

    public TestClassResult Run(TestsProps props)
    {
        uint passed = 0, failed = 0, ignored = 0, warnings = 0;

        _props = props;

        warnings += RunLifecycleMethod("BeforeAll");

        foreach (var method in TestMethods)
        {
            warnings += RunLifecycleMethod("SetUp");

            var methodResult = method.Run(props);

            warnings += RunLifecycleMethod("TearDown");

            passed += methodResult.TestsPassed;
            failed += methodResult.TestsFailed;
            ignored += methodResult.TestsIgnored;
            warnings += methodResult.Warnings;
        }

        warnings += RunLifecycleMethod("AfterAll");

        return new(passed, failed, ignored, warnings);
    }

    private void InitialLookup()
    {
        AddLifecycleMethod("BeforeAll", Reflection.GetMethodsWithAttribute<BeforeAllAttribute>(Type));
        AddLifecycleMethod("AfterAll", Reflection.GetMethodsWithAttribute<AfterAllAttribute>(Type));
        AddLifecycleMethod("SetUp", Reflection.GetMethodsWithAttribute<SetUpAttribute>(Type));
        AddLifecycleMethod("TearDown", Reflection.GetMethodsWithAttribute<TearDownAttribute>(Type));
    }

    private void AddLifecycleMethod(string name, IEnumerable<MethodInfo> methods)
    {
        if (!methods.Any()) return;

        _lifecycleMethods.Add(name, new(methods.First(), name, methods.Count() > 1));
    }

    private byte RunLifecycleMethod(string name)
    {
        if (!_lifecycleMethods.TryGetValue(name, out var method)) return 0;

        if (method.HasMultiple)
            Log.PrintWarning($"Multiple [{name}] methods found in {Type.Name}. Running only the first one.\n");

        if (_props.IsVerbose) Log.PrintLine($"[{name}] {Type.Name}");

        try
        {
            method.Method.Invoke(null, null);
        }
        catch (Exception e)
        {
            Log.PrintError($"- {e.Message}");
        }

        return method.HasMultiple ? (byte)1 : (byte)0;
    }
}
