namespace Confirma.Types;

public record ScriptMethodInfo(
    string Name,
    ScriptMethodArgumentInfo[] Args,
    string[] DefaultArgs,
    int Flags,
    int Id,
    ScriptMethodReturnInfo ReturnInfo
);
