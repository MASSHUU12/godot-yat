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
	public bool ValidatePassedData<T>(ICommand command, string[] passedData, out Dictionary<StringName, object> data) where T : CommandInputAttribute
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
					commandAttribute.Name, dataAttrArr.Select<T, string>(a => a.Name).ToArray())
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
		Dictionary<StringName, object> validatedArgs,
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
		Dictionary<StringName, object> validatedOpts,
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
		Dictionary<StringName, object> validatedArgs,
		string passedArg,
		bool log = true
	)
	{
		foreach (var type in argument.Types)
		{
			if (TryConvertStringToType(passedArg, type, out var converted))
			{
				validatedArgs[argument.Name] = converted;
				return true;
			}

			if (log) PrintNumericError(
				commandName, argument.Name,
				argument.Types, converted,
				type.Min, type.Max
			);
		}

		return false;
	}

	private bool ValidateCommandOption(
		string commandName,
		OptionAttribute option,
		Dictionary<StringName, object> validatedOpts,
		string[] passedOpts
	)
	{
		var lookup = option.Types.ToLookup(t => t.Type);

		foreach (var passedOpt in passedOpts)
		{
			if (!passedOpt.StartsWith(option.Name)) continue;

			bool isBool = lookup.Contains("bool");
			string[] tokens = passedOpt.Split('=');

			if (isBool && tokens.Length == 1)
			{
				validatedOpts[option.Name] = true;
				return true;
			}

			if ((!isBool && tokens.Length != 2)
				|| (isBool && tokens.Length != 1)
			)
			{
				Terminal.Output.Error(
					Messages.InvalidArgument(commandName, passedOpt, option.Name)
				);
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
						Terminal.Output.Error(
							Messages.InvalidArgument(commandName, passedOpt, option.Name)
						);
						return false;
					}

					List<object> convertedArr = new();

					foreach (var v in values)
					{
						if (!TryConvertStringToType(v, type, out var convertedArrValue))
						{
							PrintNumericError(
								commandName, option.Name, option.Types, convertedArrValue,
								type.Min, type.Max
							);
							return false;
						}

						convertedArr.Add(convertedArrValue);
						validatedOpts[option.Name] = convertedArr.ToArray();
					}

					return true;
				}

				if (string.IsNullOrEmpty(value))
				{
					Terminal.Output.Error(Messages.MissingValue(commandName, option.Name));
					return false;
				}

				if (!TryConvertStringToType(value, type, out var converted))
				{
					PrintNumericError(
						commandName, option.Name, option.Types, converted,
						type.Min, type.Max
					);
					return false;
				}

				validatedOpts[option.Name] = converted;
				return true;
			}
			Terminal.Output.Error(Messages.InvalidArgument(commandName, passedOpt, option.Name));
		}

		validatedOpts[option.Name] = option.DefaultValue;

		return true;
	}

	private static bool TryConvertStringToType(StringName value, CommandInputType type, out object result)
	{
		var t = (string)type.Type;
		result = null;

		GD.Print($"Trying to convert '{value}' to type '{t}'");

		if (t == "string" || t == value)
		{
			result = value;
			return true;
		}

		if (t.StartsWith("int") || t.StartsWith("float"))
		{
			var status = TryConvertNumeric(value, type, out float r);
			result = r;

			return status;
		}

		return false;
	}

	private static bool TryConvertNumeric<T>(StringName value, CommandInputType type, out T result)
	where T : notnull, IConvertible, IComparable<T>, IComparable<float>
	{
		if (!Numeric.TryConvert(value, out result)) return false;

		if (type.Min != type.Max) return Numeric.IsWithinRange(
			result, type.Min, type.Max
		);

		return true;
	}

	private void PrintNumericError(
		StringName commandName,
		StringName argumentName,
		LinkedList<CommandInputType> types,
		object value, float min, float max
	)
	{
		if (value is not null && !Numeric.IsWithinRange<float, float>((float)value, min, max))
			Terminal.Output.Error(Messages.ArgumentValueOutOfRange(
				commandName, argumentName, min, max
			));
		else if (value is null) Terminal.Output.Error(Messages.InvalidArgument(
			commandName, argumentName, string.Join(", ", types.Select(t => t.Type))
		));
	}
}
