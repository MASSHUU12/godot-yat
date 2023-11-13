using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using YAT.Attributes;
using YAT.Interfaces;

namespace YAT.Helpers
{
	public static class CommandHelper
	{
		/// <summary>
		/// Gets the arguments for the specified command.
		/// </summary>
		/// <param name="command">The command to get the arguments for.</param>
		/// <returns>A dictionary containing the arguments for the command.</returns>
		public static Dictionary<string, object> GetArguments(ICommand command)
		{
			ArgumentsAttribute attribute = command.GetAttribute<ArgumentsAttribute>();

			Dictionary<string, object> arguments = new();

			foreach (string arg in attribute.Args)
			{
				string[] parts = arg.Split(':');
				string argName = parts[0];
				string dataType = parts.Length > 1 ? parts[1] : null;

				arguments.Add(argName, ParseDataType(dataType));
			}

			return arguments;
		}

		/// <summary>
		/// Parses a string representation of a data type and returns the corresponding object or type.
		/// </summary>
		/// <param name="dataType">The string representation of the data type to parse.</param>
		/// <returns>The parsed object or type, or null if the data type could not be parsed.</returns>
		private static object ParseDataType(string dataType)
		{
			if (dataType.StartsWith("[") && dataType.EndsWith("]"))
			{
				string[] values = dataType.Trim('[', ']').Split(',').Select(v => v.Trim()).ToArray();
				return values;
			}

			Type type = GetTypeFromString(dataType);

			if (type != null) return type;

			return null;
		}

		/// <summary>
		/// Gets the type from the given string. <br />
		/// If the type is not found, it will try to find it in the "System" namespace.
		/// </summary>
		/// <param name="typeName">The name of the type.</param>
		/// <returns>The type, or null if not found.</returns>
		private static Type GetTypeFromString(string typeName)
		{
			bool isNullable = typeName.EndsWith("?");
			if (isNullable) typeName = typeName.TrimEnd('?');

			// Attempt to get the type, should be as Assembly Qualified Name.
			// If the type is not found, try with the "mscorlib" assembly
			Type type = Type.GetType(typeName, false) ??
						Type.GetType($"System.{typeName.Capitalize()}", false);

			// If the type was nullable, make it nullable
			if (isNullable && type != null) type = typeof(Nullable<>).MakeGenericType(type);

			return type;
		}
	}
}
