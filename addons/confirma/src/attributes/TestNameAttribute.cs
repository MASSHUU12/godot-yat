using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
