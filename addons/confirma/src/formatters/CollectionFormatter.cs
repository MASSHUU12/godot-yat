using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Confirma.Helpers;

namespace Confirma.Formatters;

public class CollectionFormatter(bool addTypeHint = true) : Formatter
{
    private readonly bool _addTypeHint = addTypeHint;
    private static readonly ConcurrentDictionary<Type, Type?> ElementTypeCache = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> ToStringMethodCache = new();

    public override string Format(object? value)
    {
        if (value is null)
        {
            return new DefaultFormatter().Format(value);
        }

        if (value is IEnumerable enumerable and not string)
        {
            Type collectionType = value.GetType();
            Type? elementType = GetElementType(collectionType);
            if (elementType is null)
            {
                return FormatFallback(enumerable);
            }

            MethodInfo? toStringMethod = GetToStringMethod(elementType);
            return toStringMethod is not null
                ? InvokeToStringMethod(toStringMethod, value)
                : new DefaultFormatter().Format(value);
        }

        return new DefaultFormatter().Format(value);
    }

    private static Type? GetElementType(Type collectionType)
    {
        if (ElementTypeCache.TryGetValue(collectionType, out Type? elementType))
        {
            return elementType;
        }

        elementType = collectionType.GetInterfaces()
            .FirstOrDefault(
                static t => t.IsGenericType
                && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)
            )
            ?.GetGenericArguments()[0];

        ElementTypeCache[collectionType] = elementType;

        return elementType;
    }

    private MethodInfo? GetToStringMethod(Type elementType)
    {
        if (ToStringMethodCache.TryGetValue(
            elementType,
            out MethodInfo? toStringMethod)
        )
        {
            return toStringMethod;
        }

        MethodInfo? genericMethod = typeof(CollectionHelper)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .FirstOrDefault(
                static m => m.Name == nameof(CollectionHelper.ToString)
                && m.IsGenericMethod
            );

        if (genericMethod is not null)
        {
            toStringMethod = genericMethod.MakeGenericMethod(elementType);
            ToStringMethodCache[elementType] = toStringMethod;
        }

        return toStringMethod;
    }

    private string FormatFallback(IEnumerable enumerable)
    {
        return CollectionHelper.ToString(
            enumerable.Cast<object?>(),
            depth: 0,
            maxDepth: 1,
            addBrackets: true,
            addTypeHint: _addTypeHint
        );
    }

    private string InvokeToStringMethod(MethodInfo toStringMethod, object value)
    {
        object[] parameters = new[]
        {
            value,       // The collection
            (uint)0,     // depth
            (uint)1,     // maxDepth
            true,        // addBrackets
            _addTypeHint // addTypeHint
        };
        return toStringMethod.Invoke(null, parameters) is string strResult
            ? strResult
            : new DefaultFormatter().Format(value);
    }
}
