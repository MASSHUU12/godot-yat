using System;

namespace Confirma.Enums;

[Flags]
public enum ELogOutputType : byte
{
    None = 0,
    Log = 1 << 0,
    Json = 1 << 1
}
