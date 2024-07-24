using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Confirma.Helpers;

public static class Reflection
{
    public static IEnumerable<Type> GetClassesFromAssembly(Assembly assembly)
    {
        return assembly.GetTypes().Where(type => type is
        {
            IsClass: true,
            IsAbstract: false
        });
    }

    public static IEnumerable<MethodInfo> GetMethodsWithAttribute<T>(Type type)
    where T : Attribute
    {
        return type.GetMethods().Where(method => method.CustomAttributes.Any(
            attribute => attribute.AttributeType == typeof(T)
        ));
    }

    public static bool HasAttribute<T>(this object obj) where T : Attribute
    {
        return Attribute.GetCustomAttribute(obj.GetType(), typeof(T)) is not null;
    }

    public static bool HasAttribute<T>(this Type obj) where T : Attribute
    {
        return Attribute.GetCustomAttribute(obj, typeof(T)) is not null;
    }
}
