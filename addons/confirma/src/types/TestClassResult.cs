namespace Confirma.Types;

public record TestClassResult(
    uint TestsPassed,
    uint TestsFailed,
    uint TestsIgnored,
    uint Warnings
);
