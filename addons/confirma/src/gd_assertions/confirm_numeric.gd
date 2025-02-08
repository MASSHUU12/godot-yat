@tool
class_name ConfirmNumeric

static var exts: CSharpScript = load(
	GdHelper.get_plugin_path() + "wrappers/ConfirmNumericWrapper.cs"
).new()


static func is_positive(actual: int, message: String = "") -> int:
	exts.ConfirmIsPositive(actual, message)

	return actual


static func is_not_positive(actual: int, message: String = "") -> int:
	exts.ConfirmIsNotPositive(actual, message)

	return actual


static func is_negative(actual: int, message: String = "") -> int:
	exts.ConfirmIsNegative(actual, message)

	return actual


static func is_not_negative(actual: int, message: String = "") -> int:
	exts.ConfirmIsNotNegative(actual, message)

	return actual


static func sign(actual: int, sign: bool, message: String = "") -> int:
	exts.ConfirmSign(actual, sign, message)

	return actual


static func is_zero(actual: int, message: String = "") -> int:
	exts.ConfirmIsZero(actual, message)

	return actual


static func is_not_zero(actual: int, message: String = "") -> int:
	exts.ConfirmIsNotZero(actual, message)

	return actual


static func is_odd(actual: int, message: String = "") -> int:
	exts.ConfirmIsOdd(actual, message)

	return actual


static func is_even(actual: int, message: String = "") -> int:
	exts.ConfirmIsEven(actual, message)

	return actual


static func close_to_int(
	actual: int,
	expected: int,
	tolerance: int,
	message: String = ""
) -> int:
	exts.ConfirmCloseTo(actual, expected, tolerance, message)

	return actual


static func close_to_float(
	actual: float,
	expected: float,
	tolerance: float,
	message: String = ""
) -> float:
	exts.ConfirmCloseToDouble(actual, expected, tolerance, message)

	return actual
