using System;
using System.Linq;
using System.Reflection;
using Confirma.Helpers;

namespace Confirma.Classes;

public class TestExecutor
{
	private readonly Log _log;
	private readonly Colors _colors;

	private uint _testCount, _passed, _failed;

	public TestExecutor(Log log, Colors colors)
	{
		_log = log;
		_colors = colors;
	}

	public void ExecuteTests(Assembly assembly)
	{
		var testClasses = TestDiscovery.DiscoverTestClasses(assembly);
		var count = testClasses.Count();
		var startTimeStamp = DateTime.Now;

		ResetStats();

		foreach (var testClass in testClasses)
		{
			_log.PrintLine($"Running {testClass.Type.Name}...");

			var (testsPassed, testsFailed) = testClass.Run(_log);

			_testCount += testsPassed + testsFailed;
			_passed += testsPassed;
			_failed += testsFailed;
		}

		_log.PrintLine(
			string.Format(
				"\nConfirma ran {0} tests in {1} test classes. Tests took {2}s. {3}, {4}.",
				_testCount,
				count,
				(DateTime.Now - startTimeStamp).TotalSeconds,
				_colors.Auto($"{_passed} passed", Colors.Success),
				_colors.Auto($"{_failed} failed", Colors.Error)
			)
		);
	}

	private void ResetStats()
	{
		_testCount = 0;
		_passed = 0;
		_failed = 0;
	}
}
