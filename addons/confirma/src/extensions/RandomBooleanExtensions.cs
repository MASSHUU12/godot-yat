using System;

namespace Confirma.Extensions;

public static class RandomBooleanExtensions
{
    public static bool NextBool(this Random rg)
    {
        return rg.Next(0, 2) == 1;
    }

    public static bool? NextNullableBool(this Random rg)
    {
        return rg.Next(0, 3) switch
        {
            0 => false,
            1 => true,
            _ => null,
        };
    }
}
