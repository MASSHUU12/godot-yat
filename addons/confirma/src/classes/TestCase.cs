using System;
using System.Reflection;
using Confirma.Exceptions;
using Confirma.Helpers;

namespace Confirma.Classes;

public class TestCase
{
    public MethodInfo Method { get; init; }
    public object?[]? Parameters { get; init; }
    public string Params { get; init; }
    public ushort Repeat { get; init; }

    public TestCase(MethodInfo method, object?[]? parameters, ushort repeat)
    {
        Method = method;
        Parameters = parameters;
        Params = ArrayHelper.ToString(parameters);
        Repeat = repeat;
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
