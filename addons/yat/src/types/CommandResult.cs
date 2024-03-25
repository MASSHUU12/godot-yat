using YAT.Enums;

namespace YAT.Types;

public record CommandResult(ECommandResult Result, string Message = "");
