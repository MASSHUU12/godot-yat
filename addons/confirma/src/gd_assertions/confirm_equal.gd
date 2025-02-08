@tool
class_name ConfirmEqual

static var exts: CSharpScript = load(
	GdHelper.get_plugin_path() + "wrappers/ConfirmEqualWrapper.cs"
).new()


static func equal(
	actual: Variant,
	expected: Variant,
	message: String = ""
) -> Variant:
	exts.ConfirmEqual(actual, expected, message)

	return actual


static func not_equal(
	actual: Variant,
	expected: Variant,
	message: String = ""
) -> Variant:
	exts.ConfirmNotEqual(actual, expected, message)

	return actual
