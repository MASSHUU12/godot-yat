namespace Confirma.Terminal;

public enum EArgumentParseResult : byte
{
    Success = 0,
    ValueRequired = 1 << 0,
    UnexpectedValue = 1 << 1
}
