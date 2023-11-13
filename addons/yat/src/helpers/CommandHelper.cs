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
		/// Validates the arguments passed to a command.
		/// </summary>
		/// <param name="command">The command to validate arguments for.</param>
		/// <param name="passedArgs">The arguments passed to the command.</param>
		/// <param name="arguments">The dictionary of converted arguments for the command.</param>
		/// <returns>True if the arguments are valid, false otherwise.</returns>
		public static bool ValidateCommandArguments(ICommand command, string[] passedArgs, out Dictionary<string, object> arguments)
		{
			CommandAttribute commandAttribute = command.GetAttribute<CommandAttribute>();
			var name = commandAttribute.Name;

			arguments = GetArguments(command);

			if (arguments.Count == 0) return true;

			if (passedArgs.Length < arguments.Count)
			{
				LogHelper.MissingArguments(name, arguments.Keys.ToArray());
				return false;
			}

			for (int i = 0; i < arguments.Count; i++)
			{
				string argName = arguments.Keys.ElementAt(i);
				object argType = arguments.Values.ElementAt(i);

				if (argType is string[] options)
				{
					var found = false;

					foreach (var opt in options)
					{
						if (opt == passedArgs[i])
						{
							found = true;
							arguments[argName] = opt;
							break;
						}

						object convertedArg = ConvertStringToType(opt, passedArgs[i]);

						if (convertedArg is not null)
						{
							found = true;
							arguments[argName] = convertedArg;
							break;
						}
					}

					if (!found)
					{
						LogHelper.InvalidArgument(name, argName, string.Join(", ", options));
						return false;
					}
				}
				else
				{
					object convertedArg = ConvertStringToType(
						argType?.ToString() ?? argName, passedArgs[i]
					);

					if (convertedArg is null)
					{
						LogHelper.InvalidArgument(name, argName, (string)argType ?? argName);
						return false;
					}

					arguments[argName] = convertedArg;
				}
			}

			return true;
		}

		/// <summary>
		/// Converts a string value to the specified type.
		/// </summary>
		/// <param name="type">The type to convert the value to.</param>
		/// <param name="value">The string value to convert.</param>
		/// <returns>The converted value, or null if the conversion fails.</returns>
		private static object ConvertStringToType(string type, string value)
		{
			var t = type.ToLower();

			try
			{
				if (t == "string") return value;
				if (t == "int") return int.Parse(value);
				if (t == "float") return float.Parse(value);
				if (t == "double") return double.Parse(value);
				if (t == "bool") return bool.Parse(value);
				if (t == value) return value;
			}
			catch (Exception)
			{
				return null;
			}

			return null;
		}

		/// <summary>
		/// Gets the arguments for the specified command.
		/// </summary>
		/// <param name="command">The command to get the arguments for.</param>
		/// <returns>A dictionary containing the arguments for the command.</returns>
		public static Dictionary<string, object> GetArguments(ICommand command)
		{
			ArgumentsAttribute attribute = command.GetAttribute<ArgumentsAttribute>();

			if (attribute is null) return new Dictionary<string, object>();

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
			var data = dataType?.Trim();

			if (string.IsNullOrEmpty(data)) return null;

			if (data.StartsWith("[") && data.EndsWith("]"))
			{
				string[] values = dataType.Trim('[', ']').Split(',').Select(v => v.Trim()).ToArray();
				return values;
			}

			return data;
		}

		/// <summary>
		/// Gets the type from the given string. <br />
		/// If the type is not found, it will try to find it in the "System" namespace.
		/// </summary>
		/// <param name="typeName">The name of the type.</param>
		/// <returns>The type, or null if not found.</returns>
		// private static Type GetTypeFromString(string typeName)
		// {
		// 	bool isNullable = typeName.EndsWith("?");
		// 	if (isNullable) typeName = typeName.TrimEnd('?');

		// 	// Attempt to get the type, should be as Assembly Qualified Name.
		// 	// If the type is not found, try with the "mscorlib" assembly
		// 	Type type = Type.GetType(typeName, false) ??
		// 				Type.GetType($"System.{typeName.Capitalize()}", false);

		// 	// If the type was nullable, make it nullable
		// 	if (isNullable && type != null) type = typeof(Nullable<>).MakeGenericType(type);

		// 	return type;
		// }
	}
}
