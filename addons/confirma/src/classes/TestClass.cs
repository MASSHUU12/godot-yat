using System;
using System.Collections.Generic;
using Confirma.Helpers;

namespace Confirma.Classes;

public class TestClass
{
	public Type Type { get; }
	public IEnumerable<TestMethod> TestMethods { get; }

	public TestClass(Type type)
	{
		Type = type;
		TestMethods = TestDiscovery.DiscoverTestMethods(type);
	}

	public (uint testsPassed, uint testsFailed) Run(Log log)
	{
		uint passed = 0, failed = 0;

		foreach (var method in TestMethods)
		{
			var (testsPassed, testsFailed) = method.Run(log);

			passed += testsPassed;
			failed += testsFailed;
		}

		return (passed, failed);
	}
}
