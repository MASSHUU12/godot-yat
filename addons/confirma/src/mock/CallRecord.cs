namespace Confirma.Mock;

public class CallRecord(string methodName, object?[]? arguments)
{
    public string MethodName { get; } = methodName;
    public object?[]? Arguments { get; } = arguments;
    public object? ReturnValue { get; set; }
}
