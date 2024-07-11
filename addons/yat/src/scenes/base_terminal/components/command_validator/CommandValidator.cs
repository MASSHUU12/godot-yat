using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using YAT.Attributes;
using YAT.Classes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;

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
        {
            return ValidateCommandOptions(
                    data, passedData, (dataAttrArr as OptionAttribute[])!
                );
        }

        return false;
    }

    private bool ValidateCommandArguments(
        Dictionary<StringName, object?> validatedArgs,
        string[] passedArgs,
        ArgumentAttribute[] arguments
    )
    {
        for (int i = 0; i < arguments.Length; i++)
        {
            if (!ValidateCommandArgument(arguments[i], validatedArgs, passedArgs[i]))
            {
                return false;
            }
        }

        return true;
    }

    private bool ValidateCommandOptions(
        Dictionary<StringName, object?> validatedOpts,
        string[] passedOpts,
        OptionAttribute[] options
    )
    {
        foreach (OptionAttribute opt in options)
        {
            if (!ValidateCommandOption(opt, validatedOpts, passedOpts))
            {
                return false;
            }
        }

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

        foreach (CommandType type in argument.Types)
        {
            EStringConversionResult status = Parser.TryConvertStringToType(passedArg, type, out var converted);

            if (status == EStringConversionResult.Success)
            {
                validatedArgs[argument.Name] = converted;
                return true;
            }

            if (log && index == argument.Types.Count - 1)
            {
                PrintErr(status, argument, type, converted);
            }

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
        ILookup<ECommandInputType, CommandType> lookup = option.Types.ToLookup(t => t.Type);
        bool isBool = lookup.Contains(ECommandInputType.Bool);

        foreach (string passedOpt in passedOpts)
        {
            if (!passedOpt.StartsWith(option.Name))
            {
                continue;
            }

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

            foreach (CommandType type in option.Types)
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

                    foreach (string v in values)
                    {
                        EStringConversionResult st = Parser.TryConvertStringToType(
                            v,
                            type,
                            out var convertedLValue
                        );

                        if (st != EStringConversionResult.Success)
                        {
                            PrintErr(st, option, type, convertedLValue);
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

                EStringConversionResult status = Parser.TryConvertStringToType(
                    value,
                    type,
                    out var converted
                );

                if (status != EStringConversionResult.Success)
                {
                    PrintErr(status, option, type, converted);

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

    private void PrintErr(
        EStringConversionResult status,
        CommandInputAttribute commandInput,
        CommandType commandType,
        object? value
    )
    {
        switch (status)
        {
            case EStringConversionResult.Invalid:
                Terminal.Output.Error(Messages.InvalidArgument(
                        _commandName,
                        value?.ToString() ?? commandInput.Name,
                        string.Join(", ", commandInput.Types.Select(t => t.TypeDefinition)
                    )
                ));
                break;
            case EStringConversionResult.OutOfRange:
                if (commandType is not CommandTypeRanged ranged)
                {
                    GD.PrintErr($"{commandType.GetType().Name} is not CommandTypeRanged");
                }
                else
                {
                    Terminal.Output.Error(Messages.ArgumentValueOutOfRange(
                        _commandName, commandInput.Name, ranged.Min, ranged.Max
                    ));
                }
                break;
            case EStringConversionResult.Success:
                break;
            default:
                break;
        }
    }
}
