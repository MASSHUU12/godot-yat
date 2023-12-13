using System;
using System.Reflection;

namespace YAT.Helpers
{
	public static class Reflection
	{
		/// <summary>
		/// Retrieves the events declared or inherited by the specified object.
		/// </summary>
		/// <param name="obj">The object whose events to retrieve.</param>
		/// <returns>An array of EventInfo objects representing the events declared or inherited by the specified object.</returns>
		public static EventInfo[] GetEvents(this object obj, BindingFlags bindingFlags = BindingFlags.Default)
		{
			Type type = obj.GetType();

			return type.GetEvents(bindingFlags);
		}
	}
}
