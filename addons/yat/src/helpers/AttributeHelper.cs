using System;

namespace YAT.Helpers;

public static class AttributeHelper
{
	public static T GetAttribute<T>(this object obj) where T : Attribute
	{
		if (Attribute.GetCustomAttribute(obj.GetType(), typeof(T))
			is not T attribute
		) return null;

		return attribute;
	}

	public static T[] GetAttributes<T>(this object obj) where T : Attribute
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
}
