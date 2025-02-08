class_name Ignore

enum IgnoreMode {
	ALWAYS = 0,
	IN_EDITOR = 1 << 0,
	WHEN_NOT_RUNNING_CATEGORY = 1 << 1
}

var mode: IgnoreMode
var reason: String
var hide_from_results: bool
var category: String


func _init(
	mode: IgnoreMode = IgnoreMode.ALWAYS,
	reason: String = "",
	hide_from_results: bool = false,
	category: String = ""
) -> void:
	self.mode = mode
	self.reason = reason
	self.hide_from_results = hide_from_results
	self.category = category


func is_ignored(target: RunTarget) -> bool:
	match mode:
		IgnoreMode.ALWAYS:
			return true
		IgnoreMode.IN_EDITOR:
			return Engine.is_editor_hint()
		IgnoreMode.WHEN_NOT_RUNNING_CATEGORY:
			return (
				target.target == target.RunTargetType.CATEGORY
				and target.name != category
			) or target.name == ""
		_:
			return false
