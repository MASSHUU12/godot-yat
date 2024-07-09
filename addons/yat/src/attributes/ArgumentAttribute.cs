using System;

namespace YAT.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ArgumentAttribute : CommandInputAttribute
{
    public ArgumentAttribute(string name, string type, string description = "")
    : base(name, type, description) { }
}
