@tool
class_name ConfirmString

static var exts: CSharpScript = load(
	GdHelper.get_plugin_path() + "wrappers/ConfirmStringWrapper.cs"
).new()


static func empty(actual: String, message: String = "") -> String:
	exts.ConfirmEmpty(actual, message)

	return actual


static func not_empty(actual: String, message: String = "") -> String:
	exts.ConfirmNotEmpty(actual, message)

	return actual


static func contains(
	actual: String,
	expected: String,
	message: String = ""
) -> String:
	exts.ConfirmContains(actual, expected, message)

	return actual


static func not_contains(
	actual: String,
	expected: String,
	message: String = ""
) -> String:
	exts.ConfirmNotContains(actual, expected, message)

	return actual


static func starts_with(
	actual: String,
	expected: String,
	message: String = ""
) -> String:
	exts.ConfirmStartsWith(actual, expected, message)

	return actual



static func not_starts_with(
	actual: String,
	expected: String,
	message: String = ""
) -> String:
	exts.ConfirmNotStartsWith(actual, expected, message)

	return actual


static func ends_with(
	actual: String,
	expected: String,
	message: String = ""
) -> String:
	exts.ConfirmEndsWith(actual, expected, message)

	return actual


static func not_ends_with(
	actual: String,
	expected: String,
	message: String = ""
) -> String:
	exts.ConfirmNotEndsWith(actual, expected, message)

	return actual


static func has_length(
	actual: String,
	expected: int,
	message: String = ""
) -> String:
	exts.ConfirmHasLength(actual, expected, message)

	return actual


static func not_has_length(
	actual: String,
	expected: int,
	message: String = ""
) -> String:
	exts.ConfirmNotHasLength(actual, expected, message)

	return actual


static func equals_case_insensitive(
	actual: String,
	expected: String,
	message: String = ""
) -> String:
	exts.ConfirmEqualsCaseInsensitive(actual, expected, message)

	return actual


static func not_equals_case_insensitive(
	actual: String,
	expected: String,
	message: String = ""
) -> String:
	exts.ConfirmNotEqualsCaseInsensitive(actual, expected, message)

	return actual


static func matches_pattern(
	actual: String,
	pattern: String,
	message: String = ""
) -> String:
	exts.ConfirmMatchesPattern(actual, pattern, message)

	return actual


static func does_not_match_pattern(
	actual: String,
	pattern: String,
	message: String = ""
) -> String:
	exts.ConfirmDoesNotMatchPattern(actual, pattern, message)

	return actual


static func lowercase(
	actual: String,
	message: String = ""
) -> String:
	exts.ConfirmLowercase(actual, message)

	return actual


static func uppercase(
	actual: String,
	message: String = ""
) -> String:
	exts.ConfirmUppercase(actual, message)

	return actual
