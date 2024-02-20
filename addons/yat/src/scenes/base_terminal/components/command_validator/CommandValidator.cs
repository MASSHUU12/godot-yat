using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Scenes;

public partial class CommandValidator : Node
{
	[Export] public BaseTerminal Terminal { get; set; }

	/// <summary>
	/// Validates the passed data for a given command and returns a dictionary of arguments.
	/// </summary>
	/// <typeparam name="T">The type of attribute to validate.</typeparam>
	/// <param name="command">The command to validate.</param>
	/// <param name="passedData">The arguments passed to the command.</param>
	/// <param name="data">The dictionary of arguments.</param>
	/// <returns>True if the passed data is valid, false otherwise.</returns>
	public bool ValidatePassedData<T>(ICommand command, string[] passedData, out Dictionary<string, object> data) where T : CommandInputAttribute
	{
		Type type = typeof(T);
		Type argType = typeof(ArgumentAttribute);
		Type optType = typeof(OptionAttribute);

		Debug.Assert(type == argType || type == optType);

		CommandAttribute commandAttribute = command.GetAttribute<CommandAttribute>();

		data = new();

		if (commandAttribute is null)
		{
			Terminal.Output.Error(Messages.MissingAttribute("CommandAttribute", command.GetType().Name));
			return false;
		}

		T[] dataAttrArr = command.GetType().GetCustomAttributes(type, false) as T[] ?? Array.Empty<T>();

		if (type == argType)
		{
			if (passedData.Length < dataAttrArr.Length)
			{
				Terminal.Output.Error(Messages.MissingArguments(
					commandAttribute.Name, dataAttrArr.Select(a => a.Name).ToArray())
				);
				return false;
			}

			return ValidateCommandArguments(
				commandAttribute.Name, data, passedData, dataAttrArr as ArgumentAttribute[]
			);
		}
		else if (type == optType)
			return ValidateCommandOptions(
				commandAttribute.Name, data, passedData, dataAttrArr as OptionAttribute[]
			);

		data = null;

		return false;
	}

	private bool ValidateCommandArguments(
		string commandName,
		Dictionary<string, object> validatedArgs,
		string[] passedArgs,
		ArgumentAttribute[] arguments
	)
	{
		for (int i = 0; i < arguments.Length; i++)
			if (!ValidateCommandArgument(commandName, arguments[i], validatedArgs, passedArgs[i]))
				return false;
		return true;
	}

	private bool ValidateCommandOptions(
		string commandName,
		Dictionary<string, object> validatedOpts,
		string[] passedOpts,
		OptionAttribute[] options
	)
	{
		foreach (var opt in options) if (
			!ValidateCommandOption(commandName, opt, validatedOpts, passedOpts)
		) return false;

		return true;
	}

	public bool ValidateCommandArgument(
		string commandName,
		ArgumentAttribute argument,
		Dictionary<string, object> validatedArgs,
		string passedArg,
		bool log = true
	)
	{
		foreach (var type in argument.Types)
		{
			object converted = ConvertStringToType(type, passedArg);

			if (converted is not null)
			{
				validatedArgs[argument.Name] = converted;
				return true;
			}
			else if (log)
				Terminal.Output.Error(Messages.InvalidArgument(
					commandName, argument.Name, string.Join(", ", argument.Types)
				));
		}

		return false;
	}

	private bool ValidateCommandOption(
		string commandName,
		OptionAttribute option,
		Dictionary<string, object> validatedOpts,
		string[] passedOpts
	)
	{
		var lookup = option.Types.ToLookup(t => t.Type);

		foreach (var passedOpt in passedOpts)
		{
			if (!passedOpt.StartsWith(option.Name)) continue;

			string[] tokens = passedOpt.Split('=');

			if (lookup.Contains("bool") && tokens.Length == 1)
			{
				validatedOpts[option.Name] = true;
				return true;
			}

			if ((!lookup.Contains("bool") && tokens.Length != 2)
				|| (lookup.Contains("bool") && tokens.Length != 1)
			)
			{
				Terminal.Output.Error(Messages.InvalidOption(commandName, passedOpt));
				return false;
			}

			string value = tokens[1];

			foreach (var type in option.Types)
			{
				if (type.IsArray)
				{
					string[] values = value.Split(',');

					if (values.Length == 0)
					{
						Terminal.Output.Error(Messages.InvalidOption(commandName, passedOpt));
						return false;
					}

					List<object> convertedArr = new();

					foreach (var v in values)
					{
						object convertedArrValue = ConvertStringToType(type, v);

						if (convertedArrValue is null)
						{
							Terminal.Output.Error(Messages.InvalidOption(commandName, passedOpt));
							return false;
						}

						convertedArr.Add(convertedArrValue);
						validatedOpts[option.Name] = convertedArr.ToArray();
					}
				}

				object converted = ConvertStringToType(type, value);

				if (converted is not null)
				{
					validatedOpts[option.Name] = converted;
					return true;
				}
			}
			Terminal.Output.Error(Messages.InvalidOption(commandName, passedOpt));
		}

		validatedOpts[option.Name] = option.DefaultValue;

		return true;
	}

	private static object ConvertStringToType(CommandInputType type, string value)
	{
		var t = type.Type;

		if (t == "string" || t == value) return value;
		if (t == "bool") return bool.TryParse(value, out bool result) ? result : null;

		if (t.StartsWith("int")) return TryConvertNumeric<float>(type, value);
		if (t.StartsWith("float")) return TryConvertNumeric<float>(type, value);

		return null;
	}

	private static object TryConvertNumeric<T>(CommandInputType type, string value) where T : notnull, IConvertible, IComparable<T>, IComparable<float>
	{
		if (!Numeric.TryConvert(value, out T result)) return null;

		if (type.Min != type.Max) return Numeric.IsWithinRange(
			result, type.Min, type.Max
		) ? result : null;

		return result;
	}
}
