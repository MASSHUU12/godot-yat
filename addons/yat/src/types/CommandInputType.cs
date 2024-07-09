using Godot;

namespace YAT.Types;

public record CommandInputType(StringName Type, float Min, float Max, bool IsArray)
{
    public CommandInputType() : this(string.Empty, 0.0f, 0.0f, false) { }
}
