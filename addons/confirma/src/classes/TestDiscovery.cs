using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Confirma.Attributes;
using Confirma.Helpers;

namespace Confirma.Classes;

public class TestDiscovery
{
	public static IEnumerable<Type> GetTestClassesFromAssembly(Assembly assembly)
	{
		var types = assembly.GetTypes();
		return types.Where(type => type.HasAttribute<TestClassAttribute>());
	}

	public static IEnumerable<TestingClass> GetParallelTestClasses(IEnumerable<TestingClass> classes)
	{
		return classes.Where(tc => tc.IsParallelizable);
	}

	public static IEnumerable<TestingClass> GetSequentialTestClasses(IEnumerable<TestingClass> classes)
	{
		return classes.Where(tc => !tc.IsParallelizable);
	}

	public static IEnumerable<MethodInfo> GetTestMethodsFromType(Type type)
	{
		return type.GetMethods().Where(method => method.CustomAttributes.Any(
			attribute => attribute.AttributeType == typeof(TestCaseAttribute)
		));
	}

	public static IEnumerable<TestingClass> DiscoverTestClasses(Assembly assembly)
	{
		return GetTestClassesFromAssembly(assembly)
			.Select(testClass => new TestingClass(testClass));
	}

	public static IEnumerable<TestCaseAttribute> GetTestCasesFromMethod(MethodInfo method)
	{
		return method.GetCustomAttributes<TestCaseAttribute>();
	}

	public static IEnumerable<TestingMethod> DiscoverTestMethods(Type testClass)
	{
		return GetTestMethodsFromType(testClass)
			.Select(method => new TestingMethod(method));
	}

	public static IEnumerable<TestCase> DiscoverTestCases(MethodInfo method)
	{
		return GetTestCasesFromMethod(method)
			.Select(testCase => new TestCase(method, testCase.Parameters));
	}
}
