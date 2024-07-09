namespace YAT.Attributes;

[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
public sealed class TitleAttribute : System.Attribute
{
    public string Title { get; private set; }

    public TitleAttribute(string title)
    {
        Title = title;
    }
}
