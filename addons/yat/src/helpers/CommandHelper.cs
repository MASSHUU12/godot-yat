using System;
using System.Collections.Generic;
using System.Linq;
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
		/// <param name="args">The dictionary of converted arguments for the command.</param>
		/// <returns>True if the arguments are valid, false otherwise.</returns>
		public static bool ValidateCommandArguments(ICommand command, string[] passedArgs, out Dictionary<string, object> args)
		{
			CommandAttribute commandAttribute = command.GetAttribute<CommandAttribute>();
			ArgumentsAttribute argumentsAttribute = command.GetAttribute<ArgumentsAttribute>();
			var name = commandAttribute.Name;

			if (argumentsAttribute is null)
			{
				args = new();
				return true;
			}

			args = argumentsAttribute.Args;

			if (args.Count == 0) return true;

			if (passedArgs.Length < args.Count)
			{
				LogHelper.MissingArguments(name, args.Keys.ToArray());
				return false;
			}

			for (int i = 0; i < args.Count; i++)
			{
				string argName = args.Keys.ElementAt(i);
				object argType = args.Values.ElementAt(i);

				if (argType is string[] options)
				{
					var found = false;

					foreach (var opt in options)
					{
						if (opt == passedArgs[i])
						{
							found = true;
							args[argName] = opt;
							break;
						}

						object convertedArg = ConvertStringToType(opt, passedArgs[i]);

						if (convertedArg is not null)
						{
							found = true;
							args[argName] = convertedArg;
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

					args[argName] = convertedArg;
				}
			}

			return true;
		}

		private static bool GetRange<T>(string arg, out T min, out T max) where T : IConvertible, IComparable<T>
		{
			min = default;
			max = default;

			if (!arg.Contains('(') || !arg.Contains(')')) return false;

			string[] parts = arg.Split('(', ')');
			string[] range = parts[1].Split(',');

			if (range.Length != 2) return false;

			if (!NumericHelper.TryConvert(range[0], out min)) return false;
			if (!NumericHelper.TryConvert(range[1], out max)) return false;

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

			if (t == "string") return value;
			if (t == "bool") return bool.Parse(value);
			if (t == value) return value;

			if (t.StartsWith("int")) return TryConvertNumeric<int>(type, value);
			if (t.StartsWith("float")) return TryConvertNumeric<float>(type, value);
			if (t.StartsWith("double")) return TryConvertNumeric<double>(type, value);

			return null;
		}

		/// <summary>
		/// Tries to convert a string value to a numeric type T, and returns the converted value if it is within the range specified by the type.
		/// </summary>
		/// <typeparam name="T">The numeric type to convert to.</typeparam>
		/// <param name="type">The string representation of the numeric type.</param>
		/// <param name="value">The string value to convert.</param>
		/// <returns>The converted value if it is within the range specified by the type, otherwise null.</returns>
		/// <remarks>
		/// This method uses the NumericHelper class to perform the conversion and range checking.
		/// </remarks>
		private static object TryConvertNumeric<T>(string type, string value) where T : IConvertible, IComparable<T>
		{
			if (NumericHelper.TryConvert(value, out T result))
			{
				if (GetRange(type, out T min, out T max))
				{
					return NumericHelper.IsWithinRange(result, min, max) ? result : null;
				}
				else
				{
					return result;
				}
			}

			return null;
		}
	}
}
