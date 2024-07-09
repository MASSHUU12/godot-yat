using Godot;
using Godot.Collections;

namespace YAT.Types;

public readonly struct NodeMethodInfo
{
    public StringName Name { get; }
    public Array<Dictionary<string, Variant>> Args { get; }
    public Array<Variant> DefaultArgs { get; }
    public MethodFlags Flags { get; }
    public int Id { get; }
    /// <summary>
    /// Contains the following entries:
    ///
    /// <list type="bullet">
    /// <item>name</item>
    /// <item>class_name</item>
    /// <item>type</item>
    /// <item>hint</item>
    /// <item>hint_string</item>
    /// <item>usage</item>
    /// </list>
    ///
    /// See https://docs.godotengine.org/en/stable/classes/class_object.html#class-object-method-get-property-list.
    /// </summary>
    public Dictionary<string, Variant> Return { get; }

    public NodeMethodInfo(StringName name, Array<Dictionary<string, Variant>> args, Array<Variant> defaultArgs, MethodFlags flags, int id, Dictionary<string, Variant> @return)
    {
        Name = name;
        Args = args;
        DefaultArgs = defaultArgs;
        Flags = flags;
        Id = id;
        Return = @return;
    }
}
