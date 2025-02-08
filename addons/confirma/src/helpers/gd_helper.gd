extends Node
class_name GdHelper


static var plugin_path := ""


static func get_plugin_path() -> String:
	if (!plugin_path.is_empty()):
		return plugin_path
	
	# Read the information from the project settings
	# because addons can be installed in various locations,
	# so it cannot be assumed that it will always be in the default location.
	plugin_path = ProjectSettings.get_setting("autoload/Confirma") as String
	plugin_path = plugin_path.erase(0, 1).erase(plugin_path.length() - 47, 47)
	return plugin_path


static func get_autoload_path() -> String:
	return (Confirma.get_script() as Script).resource_path.get_base_dir()
