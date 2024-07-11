using Godot;
using Godot.Collections;

namespace YAT.Helpers;

public static class World
{
    public static Dictionary? RayCast(Viewport viewport, float rayLength = 1000)
    {
        Camera3D? camera = viewport.GetCamera3D();
        Vector2 mousePos = viewport.GetMousePosition();

        if (camera is null)
        {
            return null;
        }

        Vector3 origin = camera.ProjectRayOrigin(mousePos);
        Vector3 end = origin + (camera.ProjectRayNormal(mousePos) * rayLength);
        PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(origin, end);
        query.CollideWithAreas = true;
        query.CollideWithBodies = true;

        return camera.GetWorld3D().DirectSpaceState.IntersectRay(query);
    }

    public static Node? SearchNode(Node root, string name, bool recursive = true)
    {
        return !GodotObject.IsInstanceValid(root)
            ? null
            : root.FindChild(name, recursive, false);
    }
}
