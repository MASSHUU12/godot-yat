@tool
class_name ConfirmBoolean

static var exts: CSharpScript = load(
	GdHelper.get_plugin_path() + "wrappers/ConfirmBooleanWrapper.cs"
).new()


static func is_true(actual: bool, message: String = "") -> bool:
	exts.ConfirmTrue(actual, message)

	return actual


static func is_false(actual: bool, message: String = "") -> bool:
	exts.ConfirmFalse(actual, message)

	return actual
