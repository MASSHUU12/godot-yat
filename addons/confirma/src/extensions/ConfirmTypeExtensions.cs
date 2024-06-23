using System;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmTypeExtensions
{
	public static void ConfirmType(this object? actual, Type expected, string? message = null)
	{
		if (actual?.GetType() == expected) return;

		throw new ConfirmAssertException(message ?? $"Expected object to be of type {expected}.");
	}

	public static T ConfirmType<T>(this object? actual, string? message = null)
	{
		actual.ConfirmType(typeof(T), message);
		return (T)actual!;
	}

	public static void ConfirmNotType(this object? actual, Type expected, string? message = null)
	{
		if (actual?.GetType() != expected) return;

		throw new ConfirmAssertException(message ?? $"Expected object to be not of type {expected}.");
	}

	public static void ConfirmNotType<T>(this object? actual, string? message = null)
	{
		actual.ConfirmNotType(typeof(T), message);
	}
}
