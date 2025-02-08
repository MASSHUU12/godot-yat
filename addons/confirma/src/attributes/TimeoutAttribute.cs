using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class TimeoutAttribute(uint timeout) : Attribute
{
    public uint Timeout { get; } = timeout;
}
