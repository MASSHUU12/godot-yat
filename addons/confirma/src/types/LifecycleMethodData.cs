using System.Reflection;

namespace Confirma.Types;

public record LifecycleMethodData
{
    public MethodInfo Method { get; set; }
    public string Name { get; set; }
    public bool HasMultiple { get; set; }

    public LifecycleMethodData(MethodInfo method, string name, bool hasMultiple)
    {
        Method = method;
        Name = name;
        HasMultiple = hasMultiple;
    }
}
