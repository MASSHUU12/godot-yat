using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmNullExtensions
{
	public static void ConfirmNull(this object? obj, string? message = null)
	{
		if (obj is not null)
			throw new ConfirmAssertException(message ?? "Expected object to be null.");
	}

	public static void ConfirmNotNull(this object? obj, string? message = null)
	{
		if (obj is null)
			throw new ConfirmAssertException(message ?? "Expected object to be not null.");
	}
}
