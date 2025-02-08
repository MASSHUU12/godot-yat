using System.Collections.Generic;
using Confirma.Exceptions;
using Confirma.Formatters;
using Godot;

namespace Confirma.Extensions;

public static class ConfirmDictionaryExtensions
{
    public static IDictionary<TKey, TValue> ConfirmContainsKey<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        string? message = null
    )
    {
        if (dictionary.ContainsKey(key))
        {
            return dictionary;
        }

        throw new ConfirmAssertException(
            "Expected dictionary to contain key: {1}.",
            nameof(ConfirmContainsKey),
            new AutomaticFormatter(),
            key,
            null,
            message
        );
    }

    public static IDictionary<TKey, TValue> ConfirmNotContainsKey<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        string? message = null
    )
    {
        if (!dictionary.ContainsKey(key))
        {
            return dictionary;
        }

        throw new ConfirmAssertException(
            "Expected dictionary to not contain key: {1}.",
            nameof(ConfirmNotContainsKey),
            new AutomaticFormatter(),
            key,
            null,
            message
        );
    }

    public static IDictionary<TKey, TValue> ConfirmContainsValue<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TValue value,
        string? message = null
    )
    {
        if (dictionary.Values.Contains(value))
        {
            return dictionary;
        }

        throw new ConfirmAssertException(
            "Expected dictionary to contain value: {1}.",
            nameof(ConfirmContainsValue),
            new AutomaticFormatter(),
            value,
            null,
            message
        );
    }

    public static IDictionary<TKey, TValue>
    ConfirmNotContainsValue<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TValue value,
        string? message = null
    )
    {
        if (!dictionary.Values.Contains(value))
        {
            return dictionary;
        }

        throw new ConfirmAssertException(
            "Expected dictionary to not contain value: {1}.",
            nameof(ConfirmNotContainsValue),
            new AutomaticFormatter(),
            value,
            null,
            message
        );
    }

    public static IDictionary<TKey, TValue>
    ConfirmContainsKeyValuePair<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        TValue? value,
        string? message = null
    )
    {
        if (dictionary.TryGetValue(key, out TValue? v)
            && v?.Equals(value) == true
        )
        {
            return dictionary;
        }

        throw new ConfirmAssertException(
            "Expected dictionary to contain key-value pair: {1}.",
            nameof(ConfirmContainsKeyValuePair),
            new TupleFormatter(),
            (key, value),
            null,
            message
        );
    }

    public static IDictionary<Variant, Variant>
    ConfirmContainsKeyValuePair(
        this IDictionary<Variant, Variant> dictionary,
        Variant key,
        Variant value,
        string? message = null
    )
    {
        if (dictionary.TryGetValue(key, out Variant val)
            && val.VariantEquals(value)
        )
        {
            return dictionary;
        }

        throw new ConfirmAssertException(
            "Expected dictionary to contain key-value pair: {1}.",
            nameof(ConfirmContainsKeyValuePair),
            new TupleFormatter(),
            (key, value),
            null,
            message
        );
    }

    public static IDictionary<TKey, TValue>
    ConfirmNotContainsKeyValuePair<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        TValue? value,
        string? message = null
    )
    {
        if (!dictionary.TryGetValue(key, out TValue? v)
            || v?.Equals(value) == false
        )
        {
            return dictionary;
        }

        throw new ConfirmAssertException(
            "Expected dictionary to not contain key-value pair: {1}.",
            nameof(ConfirmNotContainsKeyValuePair),
            new TupleFormatter(),
            (key, value),
            null,
            message
        );
    }

    public static IDictionary<Variant, Variant>
    ConfirmNotContainsKeyValuePair(
        this IDictionary<Variant, Variant> dictionary,
        Variant key,
        Variant value,
        string? message = null
    )
    {
        if (!dictionary.TryGetValue(key, out Variant val)
            || !val.VariantEquals(value)
        )
        {
            return dictionary;
        }

        throw new ConfirmAssertException(
            "Expected dictionary to not contain key-value pair: {1}.",
            nameof(ConfirmNotContainsKeyValuePair),
            new TupleFormatter(),
            (key, value),
            null,
            message
        );
    }
}
