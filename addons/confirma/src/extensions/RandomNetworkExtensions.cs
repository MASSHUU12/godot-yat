using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Confirma.Extensions;

public static class RandomNetworkExtensions
{
    private static readonly List<string> Domains = new() {
        "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "proton.me"
    };

    public static IPAddress NextIPAddress(this Random rg)
    {
        byte[] data = new byte[4];
        rg.NextBytes(data);

        data[0] |= 1;

        return new(data);
    }

    public static IPAddress NextIP6Address(this Random rg)
    {
        byte[] data = new byte[16];
        rg.NextBytes(data);

        data[0] |= 1;

        return new(data);
    }

    public static string NextEmail(this Random rg, int minLength = 8, int maxLength = 12)
    {
        if (minLength < 1 || maxLength < minLength)
        {
            throw new ArgumentException("Invalid length parameters");
        }

        int length = rg.Next(minLength, maxLength + 1);
        StringBuilder localPart = new(length);

        for (int i = 0; i < length; i++)
        {
            if (i < length - 1 && rg.Next(6) == 0)
            {
                _ = localPart.Append(rg.Next(2) == 0 ? '-' : '_');
            }
            else
            {
                _ = rg.Next(2) == 0
                    ? localPart.Append((char)('a' + rg.Next(26)))
                    : localPart.Append(rg.Next(10));
            }
        }

        string domain = Domains[rg.Next(Domains.Count)];
        return $"{localPart}@{domain}";
    }
}
