using System;

namespace YAT.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class OptionAttribute : CommandInputAttribute
{
    public object? DefaultValue { get; private set; }

    public OptionAttribute(string name, string type, string description = "", object? defaultValue = null)
    : base(name, type, description)
    {
        DefaultValue = defaultValue;
    }
}
