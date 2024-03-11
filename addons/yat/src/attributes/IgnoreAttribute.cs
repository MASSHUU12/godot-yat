using System;

namespace YAT.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class IgnoreAttribute : Attribute { }
