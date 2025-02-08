using System;
using System.Linq;
using Confirma.Exceptions;
using Confirma.Extensions;

namespace Confirma.Classes;

public static class Confirm
{
    public static bool IsTrue(bool expression, string? message = null)
    {
        return expression
            ? true
            : throw new ConfirmAssertException(
                message
                ?? "Expected true but was false."
            );
    }

    public static bool IsFalse(bool expression, string? message = null)
    {
        return !expression
            ? true
            : throw new ConfirmAssertException(
                message
                ?? "Expected false but was true."
            );
    }

    #region IsEnumValue
    public static int IsEnumValue<T>(int value, string? message = null)
    where T : struct, Enum
    {
        if (Enum.GetValues(typeof(T)).Cast<int>().Any(v => v == value))
        {
            return value;
        }

        throw new ConfirmAssertException(
            message
            ?? $"Expected {value} to be {typeof(T).Name} enum value."
        );
    }

    public static int IsNotEnumValue<T>(int value, string? message = null)
    where T : struct, Enum
    {
        try
        {
            _ = IsEnumValue<T>(value);
        }
        catch (ConfirmAssertException)
        {
            return value;
        }

        throw new ConfirmAssertException(
            message
            ?? $"Expected {value} not to be {typeof(T).Name} enum value."
        );
    }
    #endregion IsEnumValue

    #region IsEnumName
    public static string IsEnumName<T>(
        string name,
        bool ignoreCase = false,
        string? message = null
    )
    where T : struct, Enum
    {
        foreach (string v in Enum.GetNames(typeof(T)))
        {
            string n = v;

            if (ignoreCase)
            {
                n = n.ToLowerInvariant();
                name = name.ToLowerInvariant();
            }

            if (n == name)
            {
                return name;
            }
        }

        throw new ConfirmAssertException(
            message
            ?? $"Expected {name} to be {typeof(T).Name} enum name."
        );
    }

    public static string IsNotEnumName<T>(string name, string? message = null)
    where T : struct, Enum
    {
        try
        {
            _ = IsEnumName<T>(name);
        }
        catch (ConfirmAssertException)
        {
            return name;
        }

        throw new ConfirmAssertException(
            message
            ?? $"Expected {name} not to be {typeof(T).Name} enum name."
        );
    }
    #endregion IsEnumName

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
    #endregion Throws
}
