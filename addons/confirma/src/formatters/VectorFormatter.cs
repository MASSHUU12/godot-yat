using System;
using Godot;

namespace Confirma.Formatters;

public class VectorFormatter : Formatter
{
    public override string Format(object? value)
    {
        return value?.GetType() switch
        {
            Type t when t == typeof(Vector2)
            || t == typeof(Vector2I)
            || t == typeof(Vector3)
            || t == typeof(Vector3I)
            || t == typeof(Vector4)
            || t == typeof(Vector4I)
                => $"{t.Name}{value}",
            _ => new DefaultFormatter().Format(value),
        };
    }
}
