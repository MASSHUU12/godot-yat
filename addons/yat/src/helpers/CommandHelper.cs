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
		/// Validates the passed data for a given command and returns a dictionary of arguments.
		/// </summary>
		/// <typeparam name="T">The type of attribute to validate.</typeparam>
		/// <param name="command">The command to validate.</param>
		/// <param name="passedArgs">The arguments passed to the command.</param>
		/// <param name="args">The dictionary of arguments.</param>
		/// <returns>True if the passed data is valid, false otherwise.</returns>
		public static bool ValidatePassedData<T>(ICommand command, string[] passedArgs, out Dictionary<string, object> args) where T : Attribute
		{
			CommandAttribute commandAttribute = command.GetAttribute<CommandAttribute>();
			args = ValidateAttribute(command.GetAttribute<T>());

			if (args is null) return false;

			if (typeof(T) == typeof(ArgumentsAttribute))
			{
				if (passedArgs.Length < args.Count)
				{
					LogHelper.MissingArguments(commandAttribute.Name, args.Keys.ToArray());
					return false;
				}

				return ValidateCommandArguments(commandAttribute.Name, args, passedArgs);
			}
			else if (typeof(T) == typeof(OptionsAttribute))
				return ValidateCommandOptions(commandAttribute.Name, args, passedArgs);

			return true;
		}

		/// <summary>
		/// Validates the arguments passed to a command based on the command's attribute
		/// and the arguments dictionary.
		/// </summary>
		/// <param name="name">The command's name.</param>
		/// <param name="args">The arguments dictionary.</param>
		/// <param name="passedArgs">The arguments passed to the command.</param>
		/// <returns>True if the arguments are valid, false otherwise.</returns>
		private static bool ValidateCommandArguments(string name, Dictionary<string, object> args, string[] passedArgs)
		{
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
						LogHelper.InvalidArgument(name, argName, (string)(argType ?? argName));
						return false;
					}

					args[argName] = convertedArg;
				}
			}

			return true;
		}

		private static bool ValidateCommandOptions(string name, Dictionary<string, object> opts, string[] passedOpts)
		{
			for (int i = 0; i < opts.Count; i++)
			{
				string optName = opts.Keys.ElementAt(i);
				object optType = opts.Values.ElementAt(i);

				// By default treat the option as not passed
				opts[optName] = null;

				var passedOpt = passedOpts.Where(o => o.StartsWith(optName))
								.FirstOrDefault()
								?.Split('=', StringSplitOptions.RemoveEmptyEntries);
				string passedOptName = passedOpt?[0];
				string passedOptValue = passedOpt?.Length >= 2 ? passedOpt?[1] : null;

				// If option is a flag
				if (optType is null)
				{
					if (!string.IsNullOrEmpty(passedOptName) && string.IsNullOrEmpty(passedOptValue))
					{
						opts[optName] = true;
						continue;
					}
					opts[optName] = false;
					continue;
				}

				if (string.IsNullOrEmpty(passedOptName)) continue;

				if (string.IsNullOrEmpty(passedOptValue))
				{
					LogHelper.MissingValue(name, optName);
					return false;
				}

				// If option expects a value
				if (optType is string valueType &&
					!valueType.EndsWith("...")
				)
				{
					object convertedOpt = ConvertStringToType(
						optType.ToString(), passedOptValue
					);

					if (convertedOpt is null)
					{
						LogHelper.InvalidArgument(name, optName, (string)(optType ?? optName));
						return false;
					}

					opts[optName] = convertedOpt;
					continue;
				}

				// If option expects an array of values (type ends with ...)
				if (optType is string valuesType &&
					valuesType.EndsWith("...")
				)
				{
					string[] values = passedOptValue.Split(',',
							StringSplitOptions.TrimEntries |
							StringSplitOptions.RemoveEmptyEntries
					);
					List<object> validatedValues = new();

					foreach (var value in values)
					{
						object convertedOpt = ConvertStringToType(
							valuesType.Replace("...", string.Empty), value
						);

						if (convertedOpt is null)
						{
							LogHelper.InvalidArgument(name, optName, valuesType);
							return false;
						}

						validatedValues.Add(convertedOpt);
					}

					opts[optName] = validatedValues.ToArray();
					continue;
				}
			}

			return true;
		}

		/// <summary>
		/// Validates the given attribute and returns a dictionary of its properties.
		/// </summary>
		/// <typeparam name="T">The type of the attribute to validate.</typeparam>
		/// <param name="attribute">The attribute to validate.</param>
		/// <returns>A dictionary of the attribute's properties, or null if validation fails.</returns>
		private static Dictionary<string, object> ValidateAttribute<T>(T attribute) where T : Attribute
		{
			Dictionary<string, object> dict = new();

			if (attribute is null) return dict;

			if (attribute is ArgumentsAttribute argumentsAttribute) dict = argumentsAttribute.Args;
			else if (attribute is OptionsAttribute optionsAttribute) dict = optionsAttribute.Options;

			if (dict.Count == 0) return dict;

			return dict;
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

			if (t == "string" || t == value) return value;
			if (t == "bool") return bool.Parse(value);

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
			if (!NumericHelper.TryConvert(value, out T result)) return null;

			if (GetRange(type, out T min, out T max))
				return NumericHelper.IsWithinRange(result, min, max) ? result : null;
			return result;
		}
	}
}
