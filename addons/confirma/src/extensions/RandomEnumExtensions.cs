using System;

namespace Confirma.Extensions;

public static class RandomEnumExtensions
{
    public static int NextEnumValue<T>(this Random rg)
    where T : struct, Enum
    {
        Array values = Enum.GetValues(typeof(T));

        return (int)values.GetValue(rg.Next(0, values.Length))!;
    }

    public static string NextEnumName<T>(this Random rg)
    where T : struct, Enum
    {
        string[] names = Enum.GetNames(typeof(T));

        return names[rg.Next(0, names.Length)];
    }
}
