@tool
class_name ConfirmRange

static var exts: CSharpScript = load(
	GdHelper.get_plugin_path() + "wrappers/ConfirmRangeWrapper.cs"
).new()


static func in_range_int(
	actual: int,
	min: int,
	max: int,
	message: String = ""
) -> int:
	exts.ConfirmInRange(actual, min, max, message)

	return actual


static func in_range_float(
	actual: float,
	min: float,
	max: float,
	message: String = ""
) -> float:
	exts.ConfirmInRangeDouble(actual, min, max, message)

	return actual


static func not_in_range_int(
	actual: int,
	min: int,
	max: int,
	message: String = ""
) -> int:
	exts.ConfirmNotInRange(actual, min, max, message)

	return actual


static func not_in_range_float(
	actual: float,
	min: float,
	max: float,
	message: String = ""
) -> float:
	exts.ConfirmNotInRangeDouble(actual, min, max, message)

	return actual


static func greater_than_int(
	actual: int,
	value: int,
	message: String = ""
) -> int:
	exts.ConfirmGreaterThan(actual, value, message)

	return actual


static func greater_than_float(
	actual: float,
	value: float,
	message: String = ""
) -> float:
	exts.ConfirmGreaterThanDouble(actual, value, message)

	return actual


static func greater_than_or_equal_int(
	actual: int,
	value: int,
	message: String = ""
) -> int:
	exts.ConfirmGreaterThanOrEqual(actual, value, message)

	return actual


static func greater_than_or_equal_float(
	actual: float,
	value: float,
	message: String = ""
) -> float:
	exts.ConfirmGreaterThanOrEqualDouble(actual, value, message)

	return actual


static func less_than_int(
	actual: int,
	value: int,
	message: String = ""
) -> int:
	exts.ConfirmLessThan(actual, value, message)

	return actual


static func less_than_float(
	actual: float,
	value: float,
	message: String = ""
) -> float:
	exts.ConfirmLessThanDouble(actual, value, message)

	return actual


static func less_than_or_equal_int(
	actual: int,
	value: int,
	message: String = ""
) -> int:
	exts.ConfirmLessThanOrEqual(actual, value, message)

	return actual


static func less_than_or_equal_float(
	actual: float,
	value: float,
	message: String = ""
) -> float:
	exts.ConfirmLessThanOrEqualDouble(actual, value, message)

	return actual
