using Godot;
using static Godot.Variant;

namespace Confirma.Extensions;

public static class VariantExtensions
{
    /// <summary>
    /// Compares two Variant objects for equality.<br />
    ///
    /// Based on: https://github.com/godotengine/godot-proposals/issues/5758#issuecomment-1603407047
    /// </summary>
    /// <param name="left">The first Variant object to compare.</param>
    /// <param name="right">The second Variant object to compare.</param>
    /// <returns>True if the Variant objects are equal, false otherwise.</returns>
    public static bool VariantEquals(this Variant left, Variant right)
    {
        return left.VariantType == right.VariantType && left.VariantType switch
        {
            Type.Nil => true,
            Type.Bool => left.AsBool().Equals(right.AsBool()),
            Type.Int => left.AsInt64().Equals(right.AsInt64()),
            Type.Float => left.AsDouble().Equals(right.AsDouble()),
            Type.String => left.AsString().Equals(right.AsString()),
            Type.Vector2 => left.AsVector2().Equals(right.AsVector2()),
            Type.Vector2I => left.AsVector2I().Equals(right.AsVector2I()),
            Type.Rect2 => left.AsRect2().Equals(right.AsRect2()),
            Type.Rect2I => left.AsRect2I().Equals(right.AsRect2I()),
            Type.Vector3 => left.AsVector3().Equals(right.AsVector3()),
            Type.Vector3I => left.AsVector3I().Equals(right.AsVector3I()),
            Type.Transform2D => left.AsTransform2D().Equals(right.AsTransform2D()),
            Type.Vector4 => left.AsVector4().Equals(right.AsVector4()),
            Type.Vector4I => left.AsVector4I().Equals(right.AsVector4I()),
            Type.Plane => left.AsPlane().Equals(right.AsPlane()),
            Type.Quaternion => left.AsQuaternion().Equals(right.AsQuaternion()),
            Type.Aabb => left.AsAabb().Equals(right.AsAabb()),
            Type.Basis => left.AsBasis().Equals(right.AsBasis()),
            Type.Transform3D => left.AsTransform3D().Equals(right.AsTransform3D()),
            Type.Projection => left.AsProjection().Equals(right.AsProjection()),
            Type.Color => left.AsColor().Equals(right.AsColor()),
            Type.StringName => left.AsStringName().Equals(right.AsStringName()),
            Type.NodePath => left.AsNodePath().Equals(right.AsNodePath()),
            Type.Rid => left.AsRid().Equals(right.AsRid()),
            Type.Object => left.AsGodotObject().Equals(right.AsGodotObject()),
            Type.Callable => left.AsCallable().Equals(right),
            Type.Signal => left.AsSignal().Equals(right.AsSignal()),
            Type.Dictionary => left.AsGodotDictionary().Equals(right.AsGodotDictionary()),
            Type.Array => left.AsGodotArray().Equals(right.AsGodotArray()),
            Type.PackedByteArray => left.AsByteArray().Equals(right.AsByteArray()),
            Type.PackedInt32Array => left.AsInt32Array().Equals(right.AsInt32Array()),
            Type.PackedInt64Array => left.AsInt64Array().Equals(right.AsInt64Array()),
            Type.PackedFloat32Array => left.AsFloat32Array().Equals(right.AsFloat32Array()),
            Type.PackedFloat64Array => left.AsFloat64Array().Equals(right.AsFloat64Array()),
            Type.PackedStringArray => left.AsStringArray().Equals(right.AsStringArray()),
            Type.PackedVector2Array => left.AsVector2Array().Equals(right.AsVector2Array()),
            Type.PackedVector3Array => left.AsVector3Array().Equals(right.AsVector3Array()),
            Type.PackedColorArray => left.AsColorArray().Equals(right.AsColorArray()),
            _ => throw new System.NotSupportedException(nameof(left))
        };
    }
}
