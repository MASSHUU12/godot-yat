namespace YAT.Attributes;

[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
public sealed class DescriptionAttribute : System.Attribute
{
    public string Description { get; private set; }

    public DescriptionAttribute(string description)
    {
        Description = description;
    }
}
