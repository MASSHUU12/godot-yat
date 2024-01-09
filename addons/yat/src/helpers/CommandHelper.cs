using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YAT.Attributes;
using YAT.Interfaces;

#nullable enable
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
		public static bool ValidatePassedData<T>(ICommand command, string[] passedArgs, out Dictionary<string, object?>? args) where T : Attribute
		{
			Debug.Assert(typeof(T) == typeof(ArgumentAttribute) || typeof(T) == typeof(OptionAttribute));

			CommandAttribute? commandAttribute = command.GetAttribute<CommandAttribute>();

			if (commandAttribute is null)
			{
				Log.Error(Messages.MissingAttribute("CommandAttribute", command.GetType().Name));
				args = null;
				return false;
			}

			T[] argsArr = command.GetType().GetCustomAttributes(typeof(T), false) as T[] ?? Array.Empty<T>();

			if (typeof(T) == typeof(ArgumentAttribute))
			{
				args = argsArr.ToDictionary(a => (a as ArgumentAttribute)!.Name, a => (a as ArgumentAttribute)!.Type);
				if (passedArgs.Length < args.Count)
				{
					Log.Error(Messages.MissingArguments(commandAttribute.Name, args.Keys.ToArray()));
					return false;
				}

				return ValidateCommandArguments(commandAttribute.Name, args, passedArgs);
			}
			else if (typeof(T) == typeof(OptionAttribute))
			{
				args = argsArr.ToDictionary(
					a => (a as OptionAttribute)?.Name ?? string.Empty,
					a => (object?)new Tuple<object?, object?>(
						(a as OptionAttribute)?.Type, (a as OptionAttribute)?.DefaultValue
					)
				);

				return ValidateCommandOptions(commandAttribute.Name, args, passedArgs);
			}

			args = null;

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
		private static bool ValidateCommandArguments(string name, Dictionary<string, object?> args, string[] passedArgs)
		{
			for (int i = 0; i < args.Count; i++)
			{
				string argName = args.Keys.ElementAt(i);
				object? argType = args.Values.ElementAt(i);

				if (!ValidateCommandArgument(argName, argType, args, passedArgs[i]))
					return false;
			}

			return true;
		}

		public static bool ValidateCommandArgument(
			string name,
			object? type,
			Dictionary<string, object?> args,
			string passedArg,
			bool log = true
		)
		{
			if (type is string[] options)
			{
				var found = false;

				foreach (var opt in options)
				{
					if (opt == passedArg)
					{
						found = true;
						args[name] = opt;
						break;
					}

					object? convertedArg = ConvertStringToType(opt, passedArg);

					if (convertedArg is not null)
					{
						found = true;
						args[name] = convertedArg;
						break;
					}
				}

				if (!found)
				{
					if (log) Log.Error(
						Messages.InvalidArgument(name, name, string.Join(", ", options))
					);
					return false;
				}
			}
			else
			{
				object? convertedArg = ConvertStringToType(
					type?.ToString() ?? name, passedArg
				);

				if (convertedArg is null)
				{
					if (log) Log.Error(
						Messages.InvalidArgument(name, name, (string)(type ?? name))
					);
					return false;
				}

				args[name] = convertedArg;
			}

			return true;
		}

		/// <summary>
		/// Validates the command options based on the specified name, options, and passed options.
		/// </summary>
		/// <param name="name">The name of the command.</param>
		/// <param name="opts">The dictionary of options.</param>
		/// <param name="passedOpts">The array of passed options.</param>
		/// <returns>True if the command options are valid, false otherwise.</returns>
		private static bool ValidateCommandOptions(string name, Dictionary<string, object?> opts, string[] passedOpts)
		{
			foreach (var optEntry in opts)
			{
				string optName = optEntry.Key;
				object? optType = ((Tuple<object?, object?>?)optEntry.Value)?.Item1;

				var passedOpt = passedOpts.FirstOrDefault(o => o.StartsWith(optName), string.Empty)
								.Split('=', 2, StringSplitOptions.TrimEntries);
				string? passedOptName = passedOpt?[0];
				string? passedOptValue = passedOpt?.Length >= 2 ? passedOpt?[1] : null;

				// If option is not passed then set the option to its default value
				if (string.IsNullOrEmpty(passedOptName))
				{
					opts[optName] = ((Tuple<object?, object?>?)opts[optName])?.Item2;
					continue;
				}

				// if option is a flag (there is no type specified for the option)
				if (optType is null)
				{
					if (!string.IsNullOrEmpty(passedOptValue))
					{
						Log.Error(Messages.InvalidArgument(name, optName, optName));
						return false;
					}

					opts[optName] = !string.IsNullOrEmpty(passedOptName);
					continue;
				}

				// If option is passed but it doesn't have a value
				if (string.IsNullOrEmpty(passedOptValue))
				{
					Log.Error(Messages.MissingValue(name, optName));
					return false;
				}

				bool ProcessOptionValue(string valueType, Action<object> set)
				{
					object? converted = ConvertStringToType(valueType, passedOptValue);

					if (converted is null)
					{
						Log.Error(Messages.InvalidArgument(name, optName, valueType ?? optName));
						return false;
					}

					set(converted);

					return true;
				}

				// If option expects a value (there is no ... at the end of the type)
				if (optType is string valueType && !valueType.EndsWith("...") &&
					!valueType.Contains('|')
				)
				{
					if (ProcessOptionValue(valueType,
						(converted) => opts[optName] = converted)
					) continue;
					return false;
				}

				// If option expects an array of values (type ends with ...)
				if (optType is string valuesType && valuesType.EndsWith("..."))
				{
					string[] values = passedOptValue.Split(',',
							StringSplitOptions.TrimEntries |
							StringSplitOptions.RemoveEmptyEntries
					);
					List<object> validatedValues = new();

					foreach (var value in values)
					{
						if (!ProcessOptionValue(valuesType.Replace("...", string.Empty),
							(converted) => validatedValues.Add(converted)
						)) return false;
					}

					opts[optName] = validatedValues.ToArray();
					continue;
				}

				// If option expects one of the specified values (type contains |)
				if (optType is string optionsType && optionsType.Contains('|'))
				{
					string[] options = optionsType.Split('|');
					var found = false;

					foreach (var opt in options)
					{
						if (opt == passedOptValue)
						{
							found = true;
							opts[optName] = opt;
							break;
						}

						object? converted = ConvertStringToType(opt, passedOptValue);

						if (converted is not null)
						{
							found = true;
							opts[optName] = converted;
							break;
						}
					}

					if (!found)
					{
						Log.Error(Messages.InvalidArgument(name, optName, string.Join(", ", options)));
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Parses a string argument to extract a range of values.
		/// </summary>
		/// <typeparam name="T">The type of the values in the range.</typeparam>
		/// <param name="arg">The string argument to parse.</param>
		/// <param name="min">Contains the minimum value of the range if the parse succeeded,
		/// or the default value of <typeparamref name="T"/> if the parse failed.</param>
		/// <param name="max">Contains the maximum value of the range if the parse succeeded,
		/// or the default value of <typeparamref name="T"/> if the parse failed.</param>
		/// <returns><c>true</c> if the parse succeeded; otherwise, <c>false</c>.</returns>
		private static bool GetRange<T>(string arg, out T min, out T max) where T : notnull, IConvertible, IComparable<T>
		{
			min = default!;
			max = default!;

			if (!arg.Contains('(') || !arg.Contains(')')) return false;

			string[] parts = arg.Split('(', ')');
			string[] range = parts[1].Split(',', ':');

			if (range.Length != 2) return false;

			if (!NumericHelper.TryConvert<T>(range[0], out min)) return false;
			if (!NumericHelper.TryConvert<T>(range[1], out max)) return false;

			return true;
		}

		/// <summary>
		/// Converts a string value to the specified type.
		/// </summary>
		/// <param name="type">The type to convert the value to.</param>
		/// <param name="value">The string value to convert.</param>
		/// <returns>The converted value, or null if the conversion fails.</returns>
		private static object? ConvertStringToType(string type, string value)
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
		private static object? TryConvertNumeric<T>(string type, string value) where T : notnull, IConvertible, IComparable<T>
		{
			if (!NumericHelper.TryConvert<T>(value, out T result)) return null;

			if (GetRange(type, out T min, out T max))
				return NumericHelper.IsWithinRange(result, min, max) ? result : null;
			return result;
		}
	}
}
