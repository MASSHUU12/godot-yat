using System;

namespace Confirma.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public class RepeatAttribute : Attribute
{
    public ushort Repeat { get; init; }

    public RepeatAttribute(ushort repeat)
    {
        if (repeat == 0) throw new ArgumentOutOfRangeException(nameof(repeat));

        Repeat = (ushort)(repeat - 1);
    }
}
