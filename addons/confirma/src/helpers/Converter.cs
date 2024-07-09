using Godot;
using Godot.Collections;

namespace Confirma.Helpers;

public static class Converter
{
    public static Variant ToVariant(this object? value)
    {
        return value switch
        {
            null => default,
            bool v => v,
            long v => v,
            double v => v,
            int v => v,
            float v => v,
            char v => v,
            string v => v,
            byte[] v => v,
            int[] v => v,
            long[] v => v,
            float[] v => v,
            double[] v => v,
            string[] v => v,
            Vector2 v => v,
            Vector2I v => v,
            Rect2 v => v,
            Rect2I v => v,
            Vector3 v => v,
            Vector3I v => v,
            Transform2D v => v,
            Vector4 v => v,
            Vector4I v => v,
            Plane v => v,
            Quaternion v => v,
            Aabb v => v,
            Basis v => v,
            Transform3D v => v,
            Projection v => v,
            Color v => v,
            StringName v => v,
            NodePath v => v,
            Rid v => v,
            GodotObject v => v,
            Callable v => v,
            Signal v => v,
            Dictionary v => v,
            Array v => v,
            Vector2[] v => v,
            Vector3[] v => v,
            Color[] v => v,
            _ => throw new System.NotSupportedException(nameof(value))
        };
    }
}
