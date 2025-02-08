using System.Collections.Generic;
using Confirma.Classes;

namespace Confirma.Types;

public record TestClassResult(
    uint TestsPassed,
    uint TestsFailed,
    uint TestsIgnored,
    uint Warnings,
    List<TestLog> TestLogs
);
