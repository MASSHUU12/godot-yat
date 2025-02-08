using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestCaseAttribute(params object?[]? parameters) : Attribute
{
    public object?[]? Parameters { get; } = parameters;
}
