using System;

namespace Confirma.Extensions;

public static class RandomUuidExtensions
{
    public static Guid NextUuid4(this Random _)
    {
        return Guid.NewGuid();
    }
}
