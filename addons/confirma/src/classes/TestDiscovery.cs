using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Confirma.Helpers;

namespace Confirma.Classes;

public class TestDiscovery
{
	public static IEnumerable<TestClass> DiscoverTestClasses(Assembly assembly)
	{
		return Reflection.GetTestClassesFromAssembly(assembly)
			.Select(testClass => new TestClass(testClass));
	}

	public static IEnumerable<TestMethod> DiscoverTestMethods(Type testClass)
	{
		return Reflection.GetTestMethodsFromType(testClass)
			.Select(method => new TestMethod(method));
	}

	public static IEnumerable<TestCase> DiscoverTestCases(MethodInfo method)
	{
		return Reflection.GetTestCasesFromMethod(method)
			.Select(testCase => new TestCase(method, testCase.Parameters));
	}
}
