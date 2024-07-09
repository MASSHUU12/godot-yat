using System;
using Confirma.Exceptions;
using Confirma.Extensions;

namespace Confirma.Classes;

public static class Confirm
{
    public static bool IsTrue(bool expression, string? message = null)
    {
        if (expression) return true;

        throw new ConfirmAssertException(message ?? "Expected true but was false");
    }

    public static bool IsFalse(bool expression, string? message = null)
    {
        if (!expression) return true;

        throw new ConfirmAssertException(message ?? "Expected false but was true");
    }

    #region IsEnumValue
    public static int IsEnumValue<T>(int value, string? message = null)
    where T : struct, Enum
    {
        foreach (int v in Enum.GetValues(typeof(T))) if (v == value) return value;

        throw new ConfirmAssertException(
            message ??
            $"Expected {value} to be {typeof(T).Name} enum value."
        );
    }

    public static int IsNotEnumValue<T>(int value, string? message = null)
    where T : struct, Enum
    {
        try
        {
            IsEnumValue<T>(value);
        }
        catch (ConfirmAssertException)
        {
            return value;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected {value} not to be {typeof(T).Name} enum value."
        );
    }
    #endregion

    #region IsEnumName
    public static string IsEnumName<T>(string name, bool ignoreCase = false, string? message = null)
    where T : struct, Enum
    {
        foreach (string v in Enum.GetNames(typeof(T)))
        {
            var n = v;

            if (ignoreCase)
            {
                n = n.ToLower();
                name = name.ToLower();
            }

            if (n == name) return name;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected {name} to be {typeof(T).Name} enum name."
        );
    }

    public static string IsNotEnumName<T>(string name, string? message = null)
    where T : struct, Enum
    {
        try
        {
            IsEnumName<T>(name);
        }
        catch (ConfirmAssertException)
        {
            return name;
        }

        throw new ConfirmAssertException(
            message ??
            $"Expected {name} not to be {typeof(T).Name} enum name."
        );
    }
    #endregion

    #region Throws
    public static Action Throws<T>(Action action, string? message = null)
    where T : Exception
    {
        return action.ConfirmThrows<T>(message);
    }

    public static Action NotThrows<T>(Action action, string? message = null)
    where T : Exception
    {
        return action.ConfirmNotThrows<T>(message);
    }
    #endregion
}
