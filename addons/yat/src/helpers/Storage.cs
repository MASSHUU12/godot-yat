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
        return !ResourceLoader.Exists(path, nameof(T))
            ? null
            : ResourceLoader.Load<T>(path);
    }
}
