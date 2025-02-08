using Confirma.Enums;

namespace Confirma.Types;

public readonly record struct RunTarget(
    ERunTargetType Target = ERunTargetType.All,
    string Name = "",
    string DetailedName = ""
);
