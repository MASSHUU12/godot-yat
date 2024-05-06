using System.Collections.Generic;
using System.Reflection;
using Confirma.Attributes;
using Confirma.Exceptions;
using Confirma.Types;

using static Confirma.Enums.ETestCaseState;

namespace Confirma.Classes;

public class TestingMethod
{
	public MethodInfo Method { get; }
	public IEnumerable<TestCase> TestCases { get; }
	public string Name { get; }

	public TestingMethod(MethodInfo method)
	{
		Method = method;
		TestCases = TestDiscovery.DiscoverTestCases(method);
		Name = Method.GetCustomAttribute<TestNameAttribute>()?.Name ?? Method.Name;
	}

	public TestMethodResult Run(TestsProps props)
	{
		uint testsPassed = 0, testsFailed = 0, testsIgnored = 0;

		foreach (TestCase test in TestCases)
		{
			if (test.Method.GetCustomAttribute<IgnoreAttribute>() is IgnoreAttribute ignore)
			{
				testsIgnored++;

				TestOutput.PrintOutput(Name, test.Params, Ignored, props.IsVerbose, ignore.Reason);
				continue;
			}

			try
			{
				test.Run();
				testsPassed++;

				TestOutput.PrintOutput(Name, test.Params, Passed, props.IsVerbose);
			}
			catch (ConfirmAssertException e)
			{
				testsFailed++;

				TestOutput.PrintOutput(Name, test.Params, Failed, props.IsVerbose, e.Message);

				if (props.ExitOnFail) props.CallExitOnFailure();
			}
		}

		return new(testsPassed, testsFailed, testsIgnored);
	}
}
