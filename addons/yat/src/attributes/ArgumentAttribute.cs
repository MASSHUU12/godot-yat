using System;
using System.Linq;

namespace YAT.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ArgumentAttribute : CommandInputAttribute
{
    public ArgumentAttribute(string name, string type, string description = "")
    : base(name, type, description) { }

    public override string ToString()
    {
        string types = string.Join(" | ", Types.Select(t => t.Type));

        return $"[b]{Name}[/b]: {types} - {Description}";
    }
}
