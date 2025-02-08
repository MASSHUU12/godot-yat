using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class CategoryAttribute(string category) : Attribute
{
    public string Category { get; init; } = category;
}
