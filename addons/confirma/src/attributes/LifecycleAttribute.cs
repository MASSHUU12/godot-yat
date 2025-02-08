using System;

namespace Confirma.Attributes;

[AttributeUsage(
    AttributeTargets.Class,
    Inherited = true,
    AllowMultiple = false
)]
public class LifecycleAttribute(string className) : Attribute
{
    public string MethodName { get; init; } = className;
}
