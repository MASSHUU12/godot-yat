using YAT.Enums;

namespace YAT.Types;

public record CommandResult(
    ECommandResult Result,
    string[]? OutData = null, // TODO: Allow OutData to be an any object.
    string Message = ""
);
