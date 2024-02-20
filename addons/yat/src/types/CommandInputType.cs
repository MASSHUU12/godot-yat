using Godot;

namespace YAT.Types;

public record CommandInputType(StringName Type, float Min, float Max, bool IsArray);
