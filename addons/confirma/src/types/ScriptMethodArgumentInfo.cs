namespace Confirma.Types;

public record ScriptMethodArgumentInfo(
    string Name,
    string ClassName,
    int Type,
    int Hint,
    string HintString,
    int Usage
);
