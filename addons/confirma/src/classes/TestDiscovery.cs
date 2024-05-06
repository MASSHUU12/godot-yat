using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Confirma.Helpers;

namespace Confirma.Classes;

public class TestDiscovery
{
	public static IEnumerable<TestingClass> DiscoverTestClasses(Assembly assembly)
	{
		return Reflection.GetTestClassesFromAssembly(assembly)
			.Select(testClass => new TestingClass(testClass));
	}

	public static IEnumerable<TestingMethod> DiscoverTestMethods(Type testClass)
	{
		return Reflection.GetTestMethodsFromType(testClass)
			.Select(method => new TestingMethod(method));
	}

	public static IEnumerable<TestCase> DiscoverTestCases(MethodInfo method)
	{
		return Reflection.GetTestCasesFromMethod(method)
			.Select(testCase => new TestCase(method, testCase.Parameters));
	}
}
