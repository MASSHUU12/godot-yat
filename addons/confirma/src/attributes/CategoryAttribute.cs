using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CategoryAttribute : Attribute
{
    public string Category { get; }

    public CategoryAttribute(string category)
    {
        Category = category;
    }
}
