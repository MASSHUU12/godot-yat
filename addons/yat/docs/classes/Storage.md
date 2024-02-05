<div align="center">
	<h3>Storage</h1>
	<p>Class for managing resources.</p>
</div>

### Description

**Inherits**: N/A

Supported paths are:
-   `user://`: User-specific path that is different for each OS.
-   `res://`: Read-only path that points to the project folder.
-   `relative`: Relative paths are relative to the current working directory.
-   `absolute`: Absolute paths are absolute paths on the filesystem.

The actual directory paths for `user://` are:
-   `Windows`: %APPDATA%\Godot\app_userdata\[project_name]
-   `Linux`: ~/.local/share/godot/app_userdata/[project_name]
-   `macOS`: ~/Library/Application Support/Godot/app_userdata/[project_name]

See: https://docs.godotengine.org/en/stable/tutorials/io/data_paths.html.

### Properties

| Type | Name | Default |
| ---- | ---- | ------- |
| -    | -    | -       |

### Methods

| Type        | Definition                                                                                                     |
| ----------- | -------------------------------------------------------------------------------------------------------------- |
| static bool | SaveResource ( Resource resource, string path, ResourceSaver.SaverFlags flags = ResourceSaver.SaverFlags.None) |
| static T    | LoadResource<T> (string path)                                                                                  |

### Signals

**-**

### Enumerations

**-**
