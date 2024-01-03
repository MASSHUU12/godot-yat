using Godot;
using Godot.Collections;

namespace YAT.Helpers
{
	public static class World
	{
		/// <summary>
		/// Performs a raycast in the specified viewport and returns a dictionary containing the results.
		/// </summary>
		/// <param name="viewport">The viewport in which to perform the raycast.</param>
		/// <param name="rayLength">The length of the raycast.</param>
		/// <returns>A dictionary containing the results of the raycast.</returns>
		public static Dictionary RayCast(Viewport viewport, float rayLength = 1000)
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
	}
}
