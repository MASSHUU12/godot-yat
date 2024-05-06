using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmBooleanExtensions
{
	public static void ConfirmTrue(this bool actual, string? message = null)
	{
		if (!actual)
		{
			throw new ConfirmAssertException(message ?? "Expected true but was false.");
		}
	}

	public static void ConfirmFalse(this bool actual, string? message = null)
	{
		if (actual)
		{
			throw new ConfirmAssertException(message ?? "Expected false but was true.");
		}
	}
}
