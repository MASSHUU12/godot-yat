using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class TimeoutAttribute : Attribute
{
    public uint Timeout { get; }

    public TimeoutAttribute(uint timeout)
    {
        Timeout = timeout;
    }
}
