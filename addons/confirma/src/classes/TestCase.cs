using System;
using System.Reflection;
using Confirma.Exceptions;
using Confirma.Helpers;

namespace Confirma.Classes;

public class TestCase
{
	public MethodInfo Method { get; }
	public object?[]? Parameters { get; }
	public string Params { get; }

	public TestCase(MethodInfo method, object?[]? parameters)
	{
		Method = method;
		Parameters = parameters;
		Params = ArrayHelper.ToString(parameters);
	}

	public void Run()
	{
		try
		{
			Method.Invoke(null, Parameters);
		}
		catch (TargetInvocationException tie)
		{
			throw new ConfirmAssertException(tie.InnerException?.Message ?? tie.Message);
		}
		catch (Exception e) when (e is ArgumentException or ArgumentNullException)
		{
			throw new ConfirmAssertException($"- Failed: Invalid test case parameters: {Params}.");
		}
		catch (Exception e)
		{
			throw new ConfirmAssertException($"- Failed: {e.Message}");
		}
	}
}
