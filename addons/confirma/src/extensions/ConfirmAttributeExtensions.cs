using System;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmAttributeExtensions
{
	#region ConfirmIsDecoratedWith
	public static void ConfirmIsDecoratedWith(this Type actual, Type expected, string? message = null)
	{
		if (actual.IsDefined(expected, false)) return;

		throw new ConfirmAssertException(message ?? $"Expected '{actual.Name}' to be decorated with '{expected.Name}' but was not.");
	}

	public static void ConfirmIsDecoratedWith<T>(this Type actual, string? message = null)
		where T : Attribute
	{
		ConfirmIsDecoratedWith(actual, typeof(T), message);
	}
	#endregion

	#region ConfirmIsNotDecoratedWith
	public static void ConfirmIsNotDecoratedWith(this Type actual, Type expected, string? message = null)
	{
		if (!actual.IsDefined(expected, false)) return;

		throw new ConfirmAssertException(message ?? $"Expected '{actual.Name}' to not be decorated with '{expected.Name}' but was.");
	}

	public static void ConfirmIsNotDecoratedWith<T>(this Type actual, string? message = null)
		where T : Attribute
	{
		ConfirmIsNotDecoratedWith(actual, typeof(T), message);
	}
	#endregion
}
