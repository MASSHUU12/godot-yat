namespace YAT.Enums;

public enum ECommandResult
{
    Unavailable = -1,
    Success,
    Failure,
    InvalidArguments,
    InvalidCommand,
    InvalidPermissions,
    InvalidState,
    NotImplemented,
    UnknownCommand,
    UnknownError,
    Ok
}
