using System;
using System.Text;

namespace Confirma.Extensions;

public static class RandomStringExtensions
{
    public enum StringType { Upper = 0, Lower = 1, MixedLetter = 2, Digit = 3 }

    public static char NextChar(
        this Random rg,
        StringType type = StringType.MixedLetter
    )
    {
        return type switch
        {
            StringType.Upper => NextUpperChar(rg),
            StringType.Lower => NextLowerChar(rg),
            StringType.Digit => NextDigitChar(rg),
            _ => rg.NextBool() ? NextUpperChar(rg) : NextLowerChar(rg),
        };
    }

    public static char NextChar(this Random rg, string allowedChars)
    {
        if (!string.IsNullOrEmpty(allowedChars))
        {
            return allowedChars[rg.Next(allowedChars.Length)];
        }

        throw new ArgumentException(
            "Allowed characters cannot be null or empty."
        );
    }

    public static char NextLowerChar(this Random rg)
    {
        return (char)('a' + rg.Next(26));
    }

    public static char NextUpperChar(this Random rg)
    {
        return (char)('A' + rg.Next(26));
    }

    public static char NextDigitChar(this Random rg)
    {
        return (char)('0' + rg.Next(10));
    }

    public static string NextString(
        this Random rg,
        int minLength = 8,
        int maxLength = 12,
        StringType type = StringType.MixedLetter
    )
    {
        StringBuilder ss = new();
        int length = rg.Next(minLength, maxLength + 1);

        for (int i = 0; i < length; i++)
        {
            _ = ss.Append(rg.NextChar(type));
        }

        return ss.ToString();
    }
}
