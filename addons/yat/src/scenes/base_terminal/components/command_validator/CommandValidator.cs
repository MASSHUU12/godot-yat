using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Scenes;

public partial class CommandValidator : Node
{
#nullable disable
	[Export] public BaseTerminal Terminal { get; set; }

	private StringName _commandName;
#nullable restore

	/// <summary>
	/// Validates the passed data for a given command and returns a dictionary of arguments.
	/// </summary>
	/// <typeparam name="T">The type of attribute to validate.</typeparam>
	/// <param name="command">The command to validate.</param>
	/// <param name="passedData">The arguments passed to the command.</param>
	/// <param name="data">The dictionary of arguments.</param>
	/// <returns>True if the passed data is valid, false otherwise.</returns>
	public bool ValidatePassedData<T>(ICommand command, string[] passedData, out Dictionary<StringName, object?> data)
	where T : CommandInputAttribute
	{
		Type type = typeof(T);
		Type argType = typeof(ArgumentAttribute);
		Type optType = typeof(OptionAttribute);

		Debug.Assert(type == argType || type == optType);

		CommandAttribute commandAttribute = command.GetAttribute<CommandAttribute>()!;

		_commandName = commandAttribute.Name;
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
				data, passedData, (dataAttrArr as ArgumentAttribute[])!
			);
		}
		else if (type == optType)
			return ValidateCommandOptions(
				data, passedData, (dataAttrArr as OptionAttribute[])!
			);

		return false;
	}

	private bool ValidateCommandArguments(
		Dictionary<StringName, object?> validatedArgs,
		string[] passedArgs,
		ArgumentAttribute[] arguments
	)
	{
		for (int i = 0; i < arguments.Length; i++)
			if (!ValidateCommandArgument(arguments[i], validatedArgs, passedArgs[i]))
				return false;
		return true;
	}

	private bool ValidateCommandOptions(
		Dictionary<StringName, object?> validatedOpts,
		string[] passedOpts,
		OptionAttribute[] options
	)
	{
		foreach (var opt in options) if (
			!ValidateCommandOption(opt, validatedOpts, passedOpts)
		) return false;

		return true;
	}

	public bool ValidateCommandArgument(
		ArgumentAttribute argument,
		Dictionary<StringName, object?> validatedArgs,
		string passedArg,
		bool log = true
	)
	{
		int index = 0;

		foreach (var type in argument.Types)
		{
			var status = TryConvertStringToType(passedArg, type, out var converted);

			if (status == EStringConversionResult.Success)
			{
				validatedArgs[argument.Name] = converted;
				return true;
			}

			if (log && index == argument.Types.Count - 1)
				PrintErr(status, argument.Name, argument.Types, converted, type.Min, type.Max);
			index++;
		}

		return false;
	}

	private bool ValidateCommandOption(
		OptionAttribute option,
		Dictionary<StringName, object?> validatedOpts,
		string[] passedOpts
	)
	{
		var lookup = option.Types.ToLookup(t => t.Type);
		bool isBool = lookup.Contains("bool");

		foreach (var passedOpt in passedOpts)
		{
			if (!passedOpt.StartsWith(option.Name)) continue;

			string[] tokens = passedOpt.Split('=', 2);
			if (isBool && tokens.Length == 1)
			{
				validatedOpts[option.Name] = true;
				return true;
			}

			if (isBool && tokens.Length != 1)
			{
				Terminal.Output.Error(
					Messages.InvalidArgument(_commandName, passedOpt, option.Name)
				);
				return false;
			}

			if (tokens.Length <= 1)
			{
				Terminal.Output.Error(
					Messages.InvalidArgument(_commandName, passedOpt, string.Join(
						", ", option.Types.Select(t => t.Type + (t.IsArray ? "..." : string.Empty))
					))
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
							Messages.InvalidArgument(_commandName, passedOpt, option.Name)
						);
						return false;
					}

					List<object?> convertedL = new();

					foreach (var v in values)
					{
						var st = TryConvertStringToType(v, type, out var convertedLValue);

						if (st != EStringConversionResult.Success)
						{
							PrintErr(st, option.Name, option.Types, convertedLValue, type.Min, type.Max);
							return false;
						}

						convertedL.Add(convertedLValue);
					}
					validatedOpts[option.Name] = convertedL.ToArray();

					return true;
				}

				if (string.IsNullOrEmpty(value))
				{
					Terminal.Output.Error(Messages.MissingValue(_commandName, option.Name));
					return false;
				}

				var status = TryConvertStringToType(value, type, out var converted);

				if (status != EStringConversionResult.Success)
				{
					PrintErr(status, option.Name, option.Types, converted, type.Min, type.Max);

					return false;
				}

				validatedOpts[option.Name] = converted;
				return true;
			}
			Terminal.Output.Error(Messages.InvalidArgument(_commandName, passedOpt, option.Name));
		}

		validatedOpts[option.Name] = isBool && option.DefaultValue is null
			? false
			: option.DefaultValue;

		return true;
	}

	private static EStringConversionResult TryConvertStringToType(
		string value, CommandInputType type, out object? result
	)
	{
		var t = (string)type.Type;
		result = null;

		if (t.StartsWith("int", "float"))
		{
			var status = TryConvertNumeric(value, type, out var r);
			result = r;

			return status
				? EStringConversionResult.Success
				: EStringConversionResult.OutOfRange;
		}

		if (t == "string")
		{
			if (type.Min != type.Max && !Numeric.IsWithinRange(((string)value).Length, type.Min, type.Max)
			) return EStringConversionResult.OutOfRange;

			result = value;
			return EStringConversionResult.Success;
		}

		if (t == value)
		{
			result = value;
			return EStringConversionResult.Success;
		}

		return EStringConversionResult.Invalid;
	}

	private static bool TryConvertNumeric(StringName value, CommandInputType type, out object? result)
	{
		result = null;

		if (type.Type == "int")
		{
			if (!Numeric.TryConvert(value, out int r)) return false;

			result = r;

			if (type.Min != type.Max) return Numeric.IsWithinRange(
				r, type.Min, type.Max
			);
		}

		if (type.Type == "float")
		{
			if (!Numeric.TryConvert(value, out float r)) return false;

			result = r;

			if (type.Min != type.Max) return Numeric.IsWithinRange<float, float>(
				r, type.Min, type.Max
			);
		}

		return true;
	}

	private void PrintErr(
		EStringConversionResult status,
		StringName argumentName,
		List<CommandInputType> types,
		object? value, float min, float max
	)
	{
		switch (status)
		{
			case EStringConversionResult.Invalid:
				Terminal.Output.Error(Messages.InvalidArgument(
						_commandName,
						value?.ToString() ?? string.Empty,
						string.Join(", ", types.Select(t => t.Type)
					)
				));
				break;
			case EStringConversionResult.OutOfRange:
				Terminal.Output.Error(Messages.ArgumentValueOutOfRange(
					_commandName, argumentName, min, max
				));
				break;
		}
	}
}
