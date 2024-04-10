using System;

namespace Confirma;

public static class Confirm
{
	public static void ConfirmThrows<T>(this Action action, string? message = null)
	where T : Exception
	{
		try
		{
			action();
		}
		catch (T)
		{
			return;
		}

		throw new ConfirmAssertException(
			message
			?? $"Expected exception of type {typeof(T).Name} but no exception was thrown."
		);
	}

	public static void ConfirmThrows<T>(this Func<object> action, string? message = null)
	where T : Exception
	{
		try
		{
			action();
		}
		catch (T)
		{
			return;
		}

		throw new ConfirmAssertException(
			message
			?? $"Expected exception of type {typeof(T).Name} but no exception was thrown."
		);
	}

	public static void ConfirmNotThrows<T>(this Action action, string? message = null)
	where T : Exception
	{
		try
		{
			action();
		}
		catch (T e)
		{
			throw new ConfirmAssertException(
				message
				?? $"Expected no exception but exception of type {typeof(T).Name} was thrown: {e.Message}"
			);
		}
	}

	public static void ConfirmNotThrows<T>(this Func<object> action, string? message = null)
	where T : Exception
	{
		try
		{
			action();
		}
		catch (T e)
		{
			throw new ConfirmAssertException(
				message
				?? $"Expected no exception but exception of type {typeof(T).Name} was thrown: {e.Message}"
			);
		}
	}
}
