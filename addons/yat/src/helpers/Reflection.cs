using System;
using System.Collections.Generic;
using System.Reflection;

namespace YAT.Helpers;

public static class Reflection
{
    public static IEnumerable<EventInfo> GetEvents(
        this object obj,
        BindingFlags bindingFlags = BindingFlags.Default
    )
    {
        Type type = obj.GetType();

        return type.GetEvents(bindingFlags);
    }

    public static T? GetAttribute<T>(this object obj) where T : Attribute
    {
        return Attribute.GetCustomAttribute(obj.GetType(), typeof(T))
            is not T attribute
            ? null
            : attribute;
    }

    public static IEnumerable<T>? GetAttributes<T>(this object obj) where T : Attribute
    {
        return Attribute.GetCustomAttributes(obj.GetType(), typeof(T))
            is not T[] attributes
            ? null
            : attributes;
    }

    public static bool HasAttribute<T>(this object obj) where T : Attribute
    {
        return Attribute.GetCustomAttribute(obj.GetType(), typeof(T)) is not null;
    }

    public static bool HasInterface<T>(this object obj) where T : notnull
    {
        return obj is Type type
            ? type.HasInterface(typeof(T).FullName ?? string.Empty)
            : obj.GetType().GetInterface(typeof(T).FullName ?? string.Empty, true) is not null;
    }

    public static bool HasInterface(this Type type, string interfaceName)
    {
        return type.GetInterface(interfaceName, true) is not null;
    }
}
