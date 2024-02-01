using System;
using System.Reflection;

namespace YAT.Helpers;

public static class Reflection
{
	public static EventInfo[] GetEvents(this object obj, BindingFlags bindingFlags = BindingFlags.Default)
	{
		Type type = obj.GetType();

		return type.GetEvents(bindingFlags);
	}
}
