using Godot;

namespace YAT.Helpers;

/// <summary>
/// Helper class for saving and loading resources.<br/>
///
/// Supported path are:
/// <list type="unordered">
/// <item> user:// - The user:// path is a user-specific path that is different for each OS. </item>
/// <item> res:// - The res:// path is a read-only path that points to the project folder. </item>
/// <item> relative - Relative paths are relative to the current working directory. </item>
/// <item> absolute - Absolute paths are absolute paths on the filesystem. </item>
/// </list> <br/>
///
/// The actual directory paths for user:// are:
/// <list type="unordered">
/// <item> Windows: %APPDATA%\Godot\app_userdata\[project_name] </item>
/// <item> Linux: ~/.local/share/godot/app_userdata/[project_name] </item>
/// <item> macOS: ~/Library/Application Support/Godot/app_userdata/[project_name] </item>
/// </list>
///
/// See: https://docs.godotengine.org/en/stable/tutorials/io/data_paths.html
/// </summary>
public static class StorageHelper
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

	public static T LoadResource<T>(string path) where T : Resource
	{
		if (!ResourceLoader.Exists(path, nameof(T))) return null;

		return ResourceLoader.Load<T>(path);
	}
}
