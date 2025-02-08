@tool
class_name ConfirmVector

static var exts: CSharpScript = load(
	GdHelper.get_plugin_path() + "wrappers/ConfirmVectorWrapper.cs"
).new()


# equal_approx
static func equal_approx_2(
	vector: Vector2,
	expected: Vector2,
	tolerance: float,
	message: String = ""
) -> Vector2:
	exts.ConfirmEqualApprox(vector, expected, tolerance, message)

	return vector


static func equal_approx_3(
	vector: Vector3,
	expected: Vector3,
	tolerance: float,
	message: String = ""
) -> Vector3:
	exts.ConfirmEqualApprox(vector, expected, tolerance, message)

	return vector


static func equal_approx_4(
	vector: Vector4,
	expected: Vector4,
	tolerance: float,
	message: String = ""
) -> Vector4:
	exts.ConfirmEqualApprox(vector, expected, tolerance, message)

	return vector
# end equal_approx

# not_equal_approx
static func not_equal_approx_2(
	vector: Vector2,
	expected: Vector2,
	tolerance: float,
	message: String = ""
) -> Vector2:
	exts.ConfirmNotEqualApprox(vector, expected, tolerance, message)

	return vector


static func not_equal_approx_3(
	vector: Vector3,
	expected: Vector3,
	tolerance: float,
	message: String = ""
) -> Vector3:
	exts.ConfirmNotEqualApprox(vector, expected, tolerance, message)

	return vector


static func not_equal_approx_4(
	vector: Vector4,
	expected: Vector4,
	tolerance: float,
	message: String = ""
) -> Vector4:
	exts.ConfirmNotEqualApprox(vector, expected, tolerance, message)

	return vector
# end not_equal_approx

# less_than
static func less_than_2(
	vector: Vector2,
	expected: Vector2,
	message: String = ""
) -> Vector2:
	exts.ConfirmLessThan(vector, expected, message)

	return vector


static func less_than_3(
	vector: Vector3,
	expected: Vector3,
	message: String = ""
) -> Vector3:
	exts.ConfirmLessThan(vector, expected, message)

	return vector


static func less_than_4(
	vector: Vector4,
	expected: Vector4,
	message: String = ""
) -> Vector4:
	exts.ConfirmLessThan(vector, expected, message)

	return vector
# end less_than

# less_than_or_equal
static func less_than_or_equal_2(
	vector: Vector2,
	expected: Vector2,
	message: String = ""
) -> Vector2:
	exts.ConfirmLessThanOrEqual(vector, expected, message)

	return vector


static func less_than_or_equal_3(
	vector: Vector3,
	expected: Vector3,
	message: String = ""
) -> Vector3:
	exts.ConfirmLessThanOrEqual(vector, expected, message)

	return vector


static func less_than_or_equal_4(
	vector: Vector4,
	expected: Vector4,
	message: String = ""
) -> Vector4:
	exts.ConfirmLessThanOrEqual(vector, expected, message)

	return vector
# end less_than_or_equal

# greater_than
static func greater_than_2(
	vector: Vector2,
	expected: Vector2,
	message: String = ""
) -> Vector2:
	exts.ConfirmGreaterThan(vector, expected, message)

	return vector


static func greater_than_3(
	vector: Vector3,
	expected: Vector3,
	message: String = ""
) -> Vector3:
	exts.ConfirmGreaterThan(vector, expected, message)

	return vector


static func greater_than_4(
	vector: Vector4,
	expected: Vector4,
	message: String = ""
) -> Vector4:
	exts.ConfirmGreaterThan(vector, expected, message)

	return vector
# end greater_than

# greater_than_or_equal
static func greater_than_or_equal_2(
	vector: Vector2,
	expected: Vector2,
	message: String = ""
) -> Vector2:
	exts.ConfirmGreaterThanOrEqual(vector, expected, message)

	return vector


static func greater_than_or_equal_3(
	vector: Vector3,
	expected: Vector3,
	message: String = ""
) -> Vector3:
	exts.ConfirmGreaterThanOrEqual(vector, expected, message)

	return vector


static func greater_than_or_equal_4(
	vector: Vector4,
	expected: Vector4,
	message: String = ""
) -> Vector4:
	exts.ConfirmGreaterThanOrEqual(vector, expected, message)

	return vector
# end greater_than_or_equal

# between
static func between_2(
	vector: Vector2,
	min: Vector2,
	max: Vector2,
	message: String = ""
) -> Vector2:
	exts.ConfirmBetween(vector, min, max, message)

	return vector


static func between_3(
	vector: Vector3,
	min: Vector3,
	max: Vector3,
	message: String = ""
) -> Vector3:
	exts.ConfirmBetween(vector, min, max, message)

	return vector


static func between_4(
	vector: Vector4,
	min: Vector4,
	max: Vector4,
	message: String = ""
) -> Vector4:
	exts.ConfirmBetween(vector, min, max, message)

	return vector
# end between

# not_between
static func not_between_2(
	vector: Vector2,
	min: Vector2,
	max: Vector2,
	message: String = ""
) -> Vector2:
	exts.ConfirmNotBetween(vector, min, max, message)

	return vector


static func not_between_3(
	vector: Vector3,
	min: Vector3,
	max: Vector3,
	message: String = ""
) -> Vector3:
	exts.ConfirmNotBetween(vector, min, max, message)

	return vector


static func not_between_4(
	vector: Vector4,
	min: Vector4,
	max: Vector4,
	message: String = ""
) -> Vector4:
	exts.ConfirmNotBetween(vector, min, max, message)

	return vector
# end not_between
