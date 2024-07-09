namespace YAT.Attributes;

[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
public sealed class CommandAttribute : System.Attribute
{
    public string Name { get; private set; }
    /// <summary>
    /// Note: Supports BBCode.
    /// </summary>
    public string Description { get; private set; }
    /// <summary>
    /// Note: Supports BBCode.
    /// </summary>
    public string Manual { get; private set; }
    public string[] Aliases { get; private set; }

    public CommandAttribute(string name, string description = "", string manual = "", params string[] aliases)
    {
        Name = name;
        Description = description;
        Manual = manual;
        Aliases = aliases;
    }
}
