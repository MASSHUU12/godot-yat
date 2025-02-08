using System;
using System.Linq;
using System.Collections.Generic;

namespace Confirma.Helpers;

public static class TypeHelper
{
    public static bool ImplementsAny<T>(this Type type) where T : class
    {
        return type.ImplementsAny(typeof(T));
    }

    public static bool ImplementsAny(this Type type, params Type[] interfaces)
    {
        if (interfaces.Length == 0)
        {
            return false;
        }

        HashSet<Type> interfaceSet = new(interfaces);

        // Check if the type itself implements any of the provided interfaces
        return type.GetInterfaces().Any(
            t => IsAssignableToAny(t, interfaceSet)
        ) || (
            type.IsGenericType && interfaceSet.Any(
                i => i.IsGenericType
                    && type.GetGenericTypeDefinition()
                    == i.GetGenericTypeDefinition()
            )
        );
    }

    private static bool IsAssignableToAny(Type type, HashSet<Type> interfaces)
    {
        return interfaces.Any(i =>
        {
            if (i.IsGenericType)
            {
                return type.IsGenericType
                    ? type.GetGenericTypeDefinition() == i.GetGenericTypeDefinition()
                    : i.IsAssignableFrom(type);
            }
            return i.IsAssignableFrom(type); // For non-generic interfaces
        });
    }

    public static bool IsCollection(this Type type)
    {
        return type.ImplementsAny(
            typeof(ICollection<>),
            typeof(IEnumerable<>)
        );
    }

    public static bool IsStatic(this Type type)
    {
        return type.IsAbstract && type.IsSealed;
    }
}
