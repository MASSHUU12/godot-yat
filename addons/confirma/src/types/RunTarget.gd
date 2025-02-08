class_name RunTarget extends Resource

enum RunTargetType {
	ALL = 0,
	CLASS = 1 << 0,
	METHOD = 1 << 1,
	CATEGORY = 1 << 2
}

var target: RunTargetType = RunTargetType.ALL
var name: String = ""
var detailed_name: String = ""


# func _init(
# 	target: RunTargetType,
# 	name: String,
# 	detailed_name: String
# ) -> void:
# 	self.target = target
# 	self.name = name
# 	self.detailed_name = detailed_name
