using System.Reflection;

namespace Confirma.Types;

public record LifecycleMethodData(MethodInfo Method, string Name, bool HasMultiple)
{
    public MethodInfo Method { get; set; } = Method;
    public string Name { get; set; } = Name;
    public bool HasMultiple { get; set; } = HasMultiple;
}
