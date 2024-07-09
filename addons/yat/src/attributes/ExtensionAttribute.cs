namespace YAT.Attributes;

[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
public sealed class ExtensionAttribute : System.Attribute
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Manual { get; private set; }
    public string[] Aliases { get; private set; }

    public ExtensionAttribute(string name, string description = "", string manual = "", params string[] aliases)
    {
        Name = name;
        Description = description;
        Manual = manual;
        Aliases = aliases;
    }
}
