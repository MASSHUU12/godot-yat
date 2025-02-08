namespace Confirma.Types;

public record ScriptMethodReturnInfo(
    string Name,
    string ClassName,
    int Type,
    int Hint,
    string HintString,
    int Usage
);
