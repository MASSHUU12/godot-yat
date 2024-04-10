namespace Confirma;

public static class ConfirmEqualExtension
{
	public static void ConfirmEqual<T>(this T? actual, T? expected, string? message = null)
	{
		if (!(actual?.Equals(expected)) ?? false)
		{
			throw new ConfirmAssertException(message ?? $"Expected {expected} but was {actual}.");
		}
	}

	public static void ConfirmNotEqual<T>(this T? actual, T? expected, string? message = null)
	{
		if (actual?.Equals(expected) ?? false)
		{
			throw new ConfirmAssertException(message ?? $"Expected not {expected} but was {actual}.");
		}
	}
}
