using System;
using System.Linq;

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

    public override string ToString()
    {
        string types = string.Join(" | ", Types.Select(t => t.Type));

        return $"[b]{Name}[/b]: {types} - {Description}";
    }
}
