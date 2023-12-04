using System;

#nullable enable
namespace YAT.Helpers
{
	/// <summary>
	/// Provides helper methods for working with attributes.
	/// </summary>
	public static class AttributeHelper
	{
		/// <summary>
		/// Gets the attribute of the specified type from the object.
		/// </summary>
		/// <typeparam name="T">The type of the attribute to get.</typeparam>
		/// <param name="obj">The object to get the attribute from.</param>
		/// <returns>The attribute of the specified type, or null if the object does not have the attribute.</returns>
		public static T? GetAttribute<T>(this object obj) where T : Attribute
		{
			if (Attribute.GetCustomAttribute(obj.GetType(), typeof(T))
				is not T attribute
			) return null;

			return attribute;
		}

		/// <summary>
		/// Retrieves an array of attributes of the specified type from the given object.
		/// </summary>
		/// <typeparam name="T">The type of attribute to retrieve.</typeparam>
		/// <param name="obj">The object from which to retrieve the attributes.</param>
		/// <returns>An array of attributes of the specified type, or null if no attributes are found.</returns>
		public static T[]? GetAttributes<T>(this object obj) where T : Attribute
		{
			if (Attribute.GetCustomAttributes(obj.GetType(), typeof(T))
				is not T[] attributes
			) return null;

			return attributes;
		}
	}
}
