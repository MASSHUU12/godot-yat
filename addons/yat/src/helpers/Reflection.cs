using System;
using System.Reflection;
using Godot;

namespace YAT.Helpers;

public static class Reflection
{
	public static EventInfo[] GetEvents(this object obj, BindingFlags bindingFlags = BindingFlags.Default)
	{
		Type type = obj.GetType();

		return type.GetEvents(bindingFlags);
	}

	public static T? GetAttribute<T>(this object obj) where T : Attribute
	{
		if (Attribute.GetCustomAttribute(obj.GetType(), typeof(T))
			is not T attribute
		) return null;

		return attribute;
	}

	public static T[]? GetAttributes<T>(this object obj) where T : Attribute
	{
		if (Attribute.GetCustomAttributes(obj.GetType(), typeof(T))
			is not T[] attributes
		) return null;

		return attributes;
	}

	public static bool HasAttribute<T>(this object obj) where T : Attribute
	{
		return Attribute.GetCustomAttribute(obj.GetType(), typeof(T)) is not null;
	}

	public static bool HasInterface<T>(this object obj) where T : notnull
	{
		if (obj is null) return false;

		return obj.GetType().GetInterface(nameof(T)) is not null;
	}

	public static bool HasInterface(Type type, StringName interfaceName)
	{
		if (type is null) return false;

		return type.GetInterface(interfaceName) is not null;
	}
}
