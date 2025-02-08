@tool
class_name ConfirmDictionary

static var exts: CSharpScript = load(
	GdHelper.get_plugin_path() + "wrappers/ConfirmDictionaryWrapper.cs"
).new()


static func contains_key(
	actual: Dictionary,
	key: Variant,
	message: String = ""
) -> Dictionary:
	exts.ConfirmContainsKey(actual, key, message)

	return actual


static func not_contains_key(
	actual: Dictionary,
	key: Variant,
	message: String = ""
) -> Dictionary:
	exts.ConfirmNotContainsKey(actual, key, message)

	return actual


static func contains_value(
	actual: Dictionary,
	value: Variant,
	message: String = ""
) -> Dictionary:
	exts.ConfirmContainsValue(actual, value, message)

	return actual


static func not_contains_value(
	actual: Dictionary,
	value: Variant,
	message: String = ""
) -> Dictionary:
	exts.ConfirmNotContainsValue(actual, value, message)

	return actual


static func contains_key_value_pair(
	actual: Dictionary,
	key: Variant,
	value: Variant,
	message: String = ""
) -> Dictionary:
	exts.ConfirmContainsKeyValuePair(actual, key, value, message)

	return actual


static func not_contains_key_value_pair(
	actual: Dictionary,
	key: Variant,
	value: Variant,
	message: String = ""
) -> Dictionary:
	exts.ConfirmNotContainsKeyValuePair(actual, key, value, message)

	return actual
