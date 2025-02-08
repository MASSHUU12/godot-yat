using System;
using System.Reflection;
using Confirma.Attributes;
using Confirma.Exceptions;
using Confirma.Extensions;
using Confirma.Helpers;

namespace Confirma.Classes;

public class TestCase
{
    public MethodInfo Method { get; init; }
    public object?[]? Parameters { get; init; }
    public string Params { get; init; }
    public RepeatAttribute? Repeat { get; init; }

    public TestCase(
        MethodInfo method,
        object?[]? parameters,
        RepeatAttribute? repeat
    )
    {
        Method = method;
        Parameters = GenerateArguments(parameters);
        Params = Parameters is not null && Parameters.Length != 0
            ? CollectionHelper.ToString(Parameters, addBrackets: false)
            : string.Empty;

        Repeat = repeat;
    }

    public void Run(object? instance = null)
    {
        try
        {
            _ = Method.Invoke(instance, Parameters);
        }
        catch (TargetInvocationException tie)
        {
            throw new ConfirmAssertException(
                tie.InnerException?.Message ?? tie.Message
            );
        }
        catch (Exception e) when (e is ArgumentException or ArgumentNullException)
        {
            throw new ConfirmAssertException(
                $"- Failed: Invalid test case parameters: {Params}."
            );
        }
        catch (Exception e)
        {
            throw new ConfirmAssertException($"- Failed: {e.Message}");
        }
    }

    private object?[]? GenerateArguments(object?[]? parameters)
    {
        return Method.IsUsingParamsModifier()
            ? HandleParamsModifier(parameters)
            : parameters;
    }

    private object?[]? HandleParamsModifier(object?[]? parameters)
    {
        // This approach prevents the creation of ListPartitions
        // that create problems with assertions

        if (parameters is null)
        {
            return Array.Empty<object?>();
        }

        int numOfRegularArgs = Method.GetParameters().Length - 1;
        object?[] result = new object?[numOfRegularArgs + 1];
        Array.Copy(parameters, result, numOfRegularArgs);

        ParameterInfo paramsParameter = Method.GetParameters()[numOfRegularArgs];
        Type elementType = paramsParameter.ParameterType.GetElementType()
            ?? throw new InvalidOperationException(
                "Unable to determine the element type of the 'params' parameter."
            );

        int paramsLength = parameters.Length - numOfRegularArgs;
        Array paramsArray = Array.CreateInstance(elementType, paramsLength);
        Array.Copy(parameters, numOfRegularArgs, paramsArray, 0, paramsLength);

        result[numOfRegularArgs] = paramsArray;

        return result;
    }
}
