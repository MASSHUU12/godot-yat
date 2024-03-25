using Godot;

namespace YAT.Helpers;

public static class Storage
{
	public static bool SaveResource(
		Resource resource,
		string path,
		ResourceSaver.SaverFlags flags = ResourceSaver.SaverFlags.None
	)
	{
		return ResourceSaver.Save(resource, path, flags) switch
		{
			Error.Ok => true,
			_ => false,
		};
	}

	public static T? LoadResource<T>(string path) where T : Resource
	{
		if (!ResourceLoader.Exists(path, nameof(T))) return null;

		return ResourceLoader.Load<T>(path);
	}
}
