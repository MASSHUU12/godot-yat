using System;

namespace Confirma.Enums;

[Flags]
public enum EIgnoreMode
{
    Always = 0,
    InEditor = 1,
    WhenNotRunningCategory = 1 << 1,
    InHeadless = 1 << 2
}
