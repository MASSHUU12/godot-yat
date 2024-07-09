using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestNameAttribute : Attribute
{
    public string Name { get; }

    public TestNameAttribute(string name)
    {
        Name = name;
    }
}
