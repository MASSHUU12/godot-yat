using System;
using static System.Globalization.CultureInfo;

namespace Confirma.Helpers;

public static class EnumHelper
{
    public static bool TryParseFlagsEnum<T>(string input, out T result)
    where T : struct, Enum
    {
        result = default;

        foreach (string part in input.Split(','))
        {
            if (!Enum.TryParse(typeof(T), part, true, out object? parsed))
            {
                return false;
            }

            result = (T)Enum.ToObject(
                typeof(T),
                Convert.ToInt32(result, InvariantCulture)
                | Convert.ToInt32(parsed, InvariantCulture)
            );
        }

        return true;
    }
}
