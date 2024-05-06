using System.Collections.Generic;
using Confirma.Exceptions;
using Godot;

namespace Confirma.Extensions;

public static class ConfirmDictionaryExtensions
{
	public static void ConfirmContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, string? message = null)
	{
		if (dictionary.ContainsKey(key)) return;

		throw new ConfirmAssertException(message ?? $"Expected dictionary to contain key '{key}' but it did not.");
	}

	public static void ConfirmNotContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, string? message = null)
	{
		if (!dictionary.ContainsKey(key)) return;

		throw new ConfirmAssertException(message ?? $"Expected dictionary to not contain key '{key}' but it did.");
	}

	public static void ConfirmContainsValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value, string? message = null)
	{
		if (dictionary.Values.Contains(value)) return;

		throw new ConfirmAssertException(message ?? $"Expected dictionary to contain value '{value}' but it did not.");
	}

	public static void ConfirmNotContainsValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value, string? message = null)
	{
		if (!dictionary.Values.Contains(value)) return;

		throw new ConfirmAssertException(message ?? $"Expected dictionary to not contain value '{value}' but it did.");
	}

	public static void ConfirmContainsKeyValuePair<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, string? message = null)
	{
		if (dictionary.TryGetValue(key, out TValue? v) && v?.Equals(value) == true) return;

		throw new ConfirmAssertException(message ?? $"Expected dictionary to contain key-value pair '{key}': '{value}' but it did not.");
	}

	public static void ConfirmContainsKeyValuePair(this IDictionary<Variant, Variant> dictionary, Variant key, Variant value, string? message = null)
	{
		if (dictionary.TryGetValue(key, out var val) && val.VariantEquals(value)) return;

		throw new ConfirmAssertException(message ?? $"Expected dictionary to contain key-value pair '{key}': '{value}' but it did not.");
	}

	public static void ConfirmNotContainsKeyValuePair<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, string? message = null)
	{
		if (!dictionary.TryGetValue(key, out TValue? v) || v?.Equals(value) == false) return;

		throw new ConfirmAssertException(message ?? $"Expected dictionary to not contain key-value pair '{key}': '{value}' but it did.");
	}

	public static void ConfirmNotContainsKeyValuePair(this IDictionary<Variant, Variant> dictionary, Variant key, Variant value, string? message = null)
	{
		if (!dictionary.TryGetValue(key, out var val) || val.VariantEquals(value) == false) return;

		throw new ConfirmAssertException(message ?? $"Expected dictionary to not contain key-value pair '{key}': '{value}' but it did.");
	}
}
