using Confirma.Exceptions;
using Godot;

namespace Confirma.Extensions;

public static class ConfirmVectorExtensions
{
	#region ConfirmEqualApprox
	public static void ConfirmEqualApprox(this Vector2 vector, Vector2 expected, float tolerance = 0.0001f, string? message = null)
	{
		if (vector.IsEqualApprox(expected, tolerance)) return;

		throw new ConfirmAssertException(message ?? $"Expected {expected}, but got {vector}");
	}

	public static void ConfirmEqualApprox(this Vector3 vector, Vector3 expected, float tolerance = 0.0001f, string? message = null)
	{
		if (vector.IsEqualApprox(expected, tolerance)) return;

		throw new ConfirmAssertException(message ?? $"Expected {expected}, but got {vector}");
	}

	public static void ConfirmEqualApprox(this Vector4 vector, Vector4 expected, float tolerance = 0.0001f, string? message = null)
	{
		if (vector.IsEqualApprox(expected, tolerance)) return;

		throw new ConfirmAssertException(message ?? $"Expected {expected}, but got {vector}");
	}
	#endregion

	#region ConfirmNotEqualApprox
	public static void ConfirmNotEqualApprox(this Vector2 vector, Vector2 expected, float tolerance = 0.0001f, string? message = null)
	{
		if (!vector.IsEqualApprox(expected, tolerance)) return;

		throw new ConfirmAssertException(message ?? $"Expected not {expected}, but got {vector}");
	}

	public static void ConfirmNotEqualApprox(this Vector3 vector, Vector3 expected, float tolerance = 0.0001f, string? message = null)
	{
		if (!vector.IsEqualApprox(expected, tolerance)) return;

		throw new ConfirmAssertException(message ?? $"Expected not {expected}, but got {vector}");
	}

	public static void ConfirmNotEqualApprox(this Vector4 vector, Vector4 expected, float tolerance = 0.0001f, string? message = null)
	{
		if (!vector.IsEqualApprox(expected, tolerance)) return;

		throw new ConfirmAssertException(message ?? $"Expected not {expected}, but got {vector}");
	}
	#endregion

	#region ConfirmLessThan
	public static void ConfirmLessThan(this Vector2 vector, Vector2 expected, string? message = null)
	{
		if (vector < expected) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be less than {expected}");
	}

	public static void ConfirmLessThan(this Vector3 vector, Vector3 expected, string? message = null)
	{
		if (vector < expected) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be less than {expected}");
	}

	public static void ConfirmLessThan(this Vector4 vector, Vector4 expected, string? message = null)
	{
		if (vector < expected) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be less than {expected}");
	}
	#endregion

	#region ConfirmLessThanOrEqual
	public static void ConfirmLessThanOrEqual(this Vector2 vector, Vector2 expected, string? message = null)
	{
		if (vector <= expected) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be less than or equal to {expected}");
	}

	public static void ConfirmLessThanOrEqual(this Vector3 vector, Vector3 expected, string? message = null)
	{
		if (vector <= expected) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be less than or equal to {expected}");
	}

	public static void ConfirmLessThanOrEqual(this Vector4 vector, Vector4 expected, string? message = null)
	{
		if (vector <= expected) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be less than or equal to {expected}");
	}
	#endregion

	#region ConfirmGreaterThan
	public static void ConfirmGreaterThan(this Vector2 vector, Vector2 expected, string? message = null)
	{
		if (vector > expected) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be greater than {expected}");
	}

	public static void ConfirmGreaterThan(this Vector3 vector, Vector3 expected, string? message = null)
	{
		if (vector > expected) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be greater than {expected}");
	}

	public static void ConfirmGreaterThan(this Vector4 vector, Vector4 expected, string? message = null)
	{
		if (vector > expected) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be greater than {expected}");
	}
	#endregion

	#region ConfirmGreaterThanOrEqual
	public static void ConfirmGreaterThanOrEqual(this Vector2 vector, Vector2 expected, string? message = null)
	{
		if (vector >= expected) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be greater than or equal to {expected}");
	}

	public static void ConfirmGreaterThanOrEqual(this Vector3 vector, Vector3 expected, string? message = null)
	{
		if (vector >= expected) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be greater than or equal to {expected}");
	}

	public static void ConfirmGreaterThanOrEqual(this Vector4 vector, Vector4 expected, string? message = null)
	{
		if (vector >= expected) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be greater than or equal to {expected}");
	}
	#endregion

	#region ConfirmBetween
	public static void ConfirmBetween(this Vector2 vector, Vector2 min, Vector2 max, string? message = null)
	{
		if (vector >= min && vector <= max) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be between {min} and {max}");
	}

	public static void ConfirmBetween(this Vector3 vector, Vector3 min, Vector3 max, string? message = null)
	{
		if (vector >= min && vector <= max) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be between {min} and {max}");
	}

	public static void ConfirmBetween(this Vector4 vector, Vector4 min, Vector4 max, string? message = null)
	{
		if (vector >= min && vector <= max) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to be between {min} and {max}");
	}
	#endregion

	#region ConfirmNotBetween
	public static void ConfirmNotBetween(this Vector2 vector, Vector2 min, Vector2 max, string? message = null)
	{
		if (vector < min || vector > max) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to not be between {min} and {max}");
	}

	public static void ConfirmNotBetween(this Vector3 vector, Vector3 min, Vector3 max, string? message = null)
	{
		if (vector < min || vector > max) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to not be between {min} and {max}");
	}

	public static void ConfirmNotBetween(this Vector4 vector, Vector4 min, Vector4 max, string? message = null)
	{
		if (vector < min || vector > max) return;

		throw new ConfirmAssertException(message ?? $"Expected {vector} to not be between {min} and {max}");
	}
	#endregion
}
