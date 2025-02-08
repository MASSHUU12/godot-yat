using System;
using System.Reflection;

namespace Confirma.Extensions;

public static class MethodInfoExtensions
{
    public static bool IsUsingParamsModifier(this MethodInfo method)
    {
        ParameterInfo[] parameters = method.GetParameters();
        return parameters.Length > 0
            && parameters[^1].IsDefined(typeof(ParamArrayAttribute), false);
    }

    public static ParameterInfo? GetParamsArgument(this MethodInfo method)
    {
        ParameterInfo[] parameters = method.GetParameters();

        return method.IsUsingParamsModifier() ? parameters[^1] : null;
    }
}
