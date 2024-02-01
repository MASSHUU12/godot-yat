namespace YAT.Enums;

/// <summary>
/// Represents the result of executing a command.
/// </summary>
public enum CommandResult
{
	Success,
	Failure,
	InvalidArguments,
	InvalidCommand,
	NotImplemented,
	UnknownCommand
}
