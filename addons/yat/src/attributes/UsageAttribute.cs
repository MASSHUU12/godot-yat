namespace YAT.Attributes;

[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
public sealed class UsageAttribute : System.Attribute
{
    public string Usage { get; private set; }

    public UsageAttribute(string usage)
    {
        Usage = usage;
    }
}
