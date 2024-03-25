using Godot;
using Godot.Collections;

namespace YAT.Helpers;

public static class World
{
	public static Dictionary? RayCast(Viewport viewport, float rayLength = 1000)
	{
		var camera = viewport.GetCamera3D();
		var mousePos = viewport.GetMousePosition();

		if (camera is null) return null;

		var origin = camera.ProjectRayOrigin(mousePos);
		var end = origin + camera.ProjectRayNormal(mousePos) * rayLength;
		var query = PhysicsRayQueryParameters3D.Create(origin, end);
		query.CollideWithAreas = true;
		query.CollideWithBodies = true;

		return camera.GetWorld3D().DirectSpaceState.IntersectRay(query);
	}

	public static Node? SearchNode(Node root, string name, bool recursive = true)
	{
		if (!GodotObject.IsInstanceValid(root)) return null;

		return root.FindChild(name, recursive, false);
	}
}
