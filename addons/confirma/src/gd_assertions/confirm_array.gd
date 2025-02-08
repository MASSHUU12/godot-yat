@tool
class_name ConfirmArray

static var exts: CSharpScript = load(
	GdHelper.get_plugin_path() + "wrappers/ConfirmArrayWrapper.cs"
).new()


static func is_of_size(actual: Array, expected: int, message: String = "") -> Array:
	exts.ConfirmSize(actual, expected, message)

	return actual


static func is_empty(actual: Array, message: String = "") -> Array:
	exts.ConfirmEmpty(actual, message)

	return actual


static func is_not_empty(actual: Array, message: String = "") -> Array:
	exts.ConfirmNotEmpty(actual, message)

	return actual


static func contains(actual: Array, expected: Variant, message: String = "") -> Array:
	exts.ConfirmContains(actual, expected, message)

	return actual


static func not_contains(actual: Array, expected: Variant, message: String = "") -> Array:
	exts.ConfirmNotContains(actual, expected, message)

	return actual
