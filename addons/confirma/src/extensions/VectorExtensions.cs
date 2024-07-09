using Godot;

namespace Confirma.Extensions;

public static class VectorExtensions
{
    public static bool IsEqualApprox(this Vector2 vector, Vector2 vector2, float tolerance = 0.0001f)
    {
        return (vector - vector2).Length() < tolerance;
    }

    public static bool IsEqualApprox(this Vector3 vector, Vector3 vector2, float tolerance = 0.0001f)
    {
        return (vector - vector2).Length() < tolerance;
    }

    public static bool IsEqualApprox(this Vector4 vector, Vector4 vector2, float tolerance = 0.0001f)
    {
        return (vector - vector2).Length() < tolerance;
    }
}
