using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Confirma.Attributes;
using Confirma.Helpers;
using Confirma.Types;

namespace Confirma.Classes;

public static class TestExecutor
{
	public static TestsProps Props
	{
		get => _props;
		set
		{
			_props.ExitOnFailure -= () => { };

			_props = value;

			_props.ExitOnFailure += () =>
			{
				// GetTree().Quit() doesn't close the program immediately
				// and allows all the remaining tests to run.
				// This is a workaround to close the program immediately,
				// at the cost of Godot displaying a lot of errors.
				Environment.Exit(1);
			};
		}
	}

	private static TestsProps _props;
	private static readonly object _lock = new();

	public static void ExecuteTests(Assembly assembly, string className)
	{
		var testClasses = TestDiscovery.DiscoverTestClasses(assembly);
		var startTimeStamp = DateTime.Now;

		if (!string.IsNullOrEmpty(className))
		{
			testClasses = testClasses.Where(tc => tc.Type.Name == className);

			if (!testClasses.Any())
			{
				Log.PrintError($"No test class found with the name '{className}'.");
				return;
			}
		}

		_props.ResetStats();

		if (_props.DisableParallelization)
			foreach (var testClass in testClasses) ExecuteSingleClass(testClass);
		else
		{
			var (parallelTestClasses, sequentialTestClasses) = ClassifyTests(testClasses);

			parallelTestClasses.AsParallel().ForAll(ExecuteSingleClass);

			foreach (var testClass in sequentialTestClasses)
				ExecuteSingleClass(testClass);
		}

		PrintSummary(testClasses.Count(), startTimeStamp);
	}

	private static (IEnumerable<TestingClass>, IEnumerable<TestingClass>) ClassifyTests(IEnumerable<TestingClass> tests)
	{
		return (
		  Reflection.GetParallelTestClasses(tests),
		  Reflection.GetSequentialTestClasses(tests)
		);
	}

	private static void ExecuteSingleClass(TestingClass testClass)
	{
		lock (_lock)
		{
			Log.Print($"> {testClass.Type.Name}...");

			if (testClass.Type.GetCustomAttribute<IgnoreAttribute>() is IgnoreAttribute ignore)
			{
				_props.Result.TestsIgnored += (uint)testClass.TestMethods.Sum(m => m.TestCases.Count());

				Log.PrintWarning($" ignored.\n");
				if (ignore.Reason is not null) Log.PrintWarning($"- {ignore.Reason}\n");
				return;
			}

			Log.PrintLine();

			var classResult = testClass.Run(_props);

			_props.Result += classResult;
		}
	}

	private static void PrintSummary(int count, DateTime startTimeStamp)
	{
		Log.PrintLine(
			string.Format(
				"\nConfirma ran {0} tests in {1} test classes. Tests took {2}s.\n{3}, {4}, {5}, {6}.",
				_props.Result.TotalTests,
				count,
				(DateTime.Now - startTimeStamp).TotalSeconds,
				Colors.ColorText($"{_props.Result.TestsPassed} passed", Colors.Success),
				Colors.ColorText($"{_props.Result.TestsFailed} failed", Colors.Error),
				Colors.ColorText($"{_props.Result.TestsIgnored} ignored", Colors.Warning),
				Colors.ColorText($"{_props.Result.Warnings} warnings", Colors.Warning)
			)
		);
	}
}
