using System;
using Confirma.Exceptions;

namespace Confirma.Extensions;

public static class ConfirmExceptionExtensions
{
	#region ConfirmThrows
	public static void ConfirmThrows<T>(this Func<T> action, Type e, string? message = null)
	{
		try
		{
			action();
		}
		catch (Exception ex)
		{
			if (ex.GetType() == e) return;

			throw new ConfirmAssertException(
				message
				?? $"Expected exception of type '{e.Name}' but exception of type '{ex.GetType().Name}' was thrown."
			);
		}

		throw new ConfirmAssertException(
			message
			?? $"Expected exception of type '{e.Name}' but no exception was thrown."
		);
	}

	public static void ConfirmThrows<E>(this Func<object?> action, string? message = null)
	where E : Exception
	{
		action.ConfirmThrows(typeof(E), message);
	}

	public static void ConfirmThrows<E>(this Action action, string? message = null)
	{
		Func<object> func = () =>
		{
			action();
			return new object();
		};

		func.ConfirmThrows(typeof(E), message);
	}
	#endregion

	#region ConfirmNotThrows
	public static void ConfirmNotThrows<T>(this Func<T> action, Type e, string? message = null)
	{
		try
		{
			action();
		}
		catch (Exception ex)
		{
			if (ex.GetType() == e)
			{
				throw new ConfirmAssertException(
					message
					?? $"Expected exception of type '{e.Name}' not to be thrown but it was."
				);
			}
		}
	}

	public static void ConfirmNotThrows<E>(this Func<object?> action, string? message = null)
	where E : Exception
	{
		ConfirmNotThrows(action, typeof(E), message);
	}

	public static void ConfirmNotThrows<E>(this Action action, string? message = null)
	{
		Func<object> func = () =>
		{
			action();
			return new object();
		};

		func.ConfirmNotThrows(typeof(E), message);
	}
	#endregion
}
