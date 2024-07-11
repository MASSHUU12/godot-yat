using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using YAT.Enums;
using YAT.Helpers;
using static YAT.Enums.ECommandInputType;

namespace YAT.Classes;

public static class Parser
{
    public static string[] ParseCommand(string command)
    {
        string[] tokens = Text.SanitizeText(command);

        return Text.ConcatenateSentence(tokens);
    }

    public static (string, string[]) ParseMethod(string method)
    {
        string[] tokens = Text.SanitizeText(method);
        List<string> args = new();

        if (tokens.Length == 0)
        {
            return (string.Empty, Array.Empty<string>());
        }

        string[] parts = (tokens.Length <= 1 ? tokens[^1] : tokens[0])
            .Split(
                '(',
                2,
                StringSplitOptions.RemoveEmptyEntries
                | StringSplitOptions.TrimEntries
            );

        if (parts.Length > 1 && !string.IsNullOrEmpty(parts[1][..^1]))
        {
            args.Add(parts[1][..^1]);
        }

        if (tokens.Length >= 1)
        {
            for (int i = 1; i < tokens.Length; i++)
            {
                args.Add(tokens[i][..^1]);
            }
        }

        return (parts[0], args.ToArray());
    }

    public static bool TryParseStringTypeToEnum(string type, out ECommandInputType parsed)
    {
        parsed = ECommandInputType.Void;

        if (System.Enum.TryParse(typeof(ECommandInputType), type, true, out var result))
        {
            parsed = (ECommandInputType)result;
            return true;
        }

        return false;
    }

    public static bool TryParseCommandInputType(string stringToParse, out CommandType parsed)
    {
        parsed = new();
        stringToParse = stringToParse.Trim();

        static bool AllowedToHaveRange(ECommandInputType type)
        {
            return type is Int or Float or ECommandInputType.String;
        }

        if (string.IsNullOrEmpty(stringToParse))
        {
            return false;
        }

        bool isArray = stringToParse.EndsWith("...");
        if (isArray)
        {
            stringToParse = stringToParse[..^3].Trim();
        }

        // Get the min and max values if present
        var tokens = stringToParse.Trim(')').Split('(', StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length is 0 or > 2)
        {
            return false;
        }

        if ((!TryParseStringTypeToEnum(tokens[0], out var enumType) || enumType == Constant) && tokens.Length == 1)
        {
            parsed = new(tokens[0], Constant, isArray);
            return true;
        }

        if (enumType == ECommandInputType.Void)
        {
            return false;
        }

        if (enumType == ECommandInputType.Enum)
        {
            if (tokens.Length < 2)
            {
                return false;
            }

            if (!TryParseEnumType(tokens[1].Split(','), isArray, out var result))
            {
                return false;
            }

            parsed = result!;

            return true;
        }

        if (AllowedToHaveRange(enumType))
        {
            if (tokens.Length > 1) // Type with range
            {
                if (!TryParseTypeWithRange(enumType, tokens[1], isArray, out var result))
                {
                    return false;
                }

                parsed = result;
            }
            else
            {
                parsed = new CommandTypeRanged(tokens[0], enumType, isArray);
            }
        }
        else
        {
            parsed = new(tokens[0], enumType, isArray);
        }

        return true;
    }

    public static bool TryParseEnumType(
        IEnumerable<string> tokens,
        bool isArray,
        out CommandTypeEnum? parsed
    )
    {
        Dictionary<string, int> result = new();

        parsed = null;

        if (!tokens.Any())
        {
            return false;
        }

        foreach (string token in tokens)
        {
            string[] parts = token.Split(':', StringSplitOptions.TrimEntries);

            if (parts.Length != 2)
            {
                return false;
            }

            if (!int.TryParse(parts[1], out var value))
            {
                return false;
            }

            result.Add(parts[0], value);
        }

        parsed = new("Lorem", isArray, result); // TODO: Give actual name

        return true;
    }

    public static bool TryParseTypeWithRange(
        ECommandInputType type,
        string range,
        bool isArray,
        out CommandTypeRanged parsed
    )
    {
        static bool AllowedToHaveRange(ECommandInputType type)
        {
            return type is Int or Float or ECommandInputType.String;
        }

        CommandTypeRanged CreateOut(float min = float.MinValue, float max = float.MaxValue)
        {
            return new(nameof(type), type, isArray, min, max); // TODO: Give actual name
        }

        parsed = new();

        if (!string.IsNullOrEmpty(range) && !AllowedToHaveRange(type))
        {
            return false;
        }

        // If range is missing
        if (string.IsNullOrEmpty(range))
        {
            parsed = CreateOut();

            return true;
        }

        ushort colonCount = (ushort)range.Count(c => c == ':');

        if (colonCount > 1)
        {
            return false;
        }

        bool maxPresent = !range.EndsWith(':');
        var minMax = range.Split(':', StringSplitOptions.RemoveEmptyEntries);

        // If range is invalid return
        if (minMax.Length is 0 or > 2)
        {
            return false;
        }

        // If only one value was passed (min or max)
        if (minMax.Length == 1)
        {
            if (!minMax[0].TryConvert(out float val))
            {
                return false;
            }

            parsed = maxPresent
                ? CreateOut(max: val)
                : CreateOut(min: val);

            return parsed.Min >= float.MinValue && parsed.Max <= float.MaxValue;
        }

        // If both values were passed
        if (minMax[0].TryConvert(out float min) && minMax[1].TryConvert(out float max))
        {
            parsed = CreateOut(min, max);
            return min < max;
        }

        return false;
    }

    public static EStringConversionResult TryConvertStringToType(
        string value, CommandType commandType, out object? result
    )
    {
        result = null;

        if (string.IsNullOrEmpty(value))
        {
            return EStringConversionResult.Invalid;
        }

        ECommandInputType type = commandType.Type;

        if (type == ECommandInputType.Void)
        {
            return EStringConversionResult.Invalid;
        }

        if (commandType is CommandTypeRanged ranged)
        {
            if (type == ECommandInputType.String)
            {
                if (!Numeric.IsWithinRange(value.Length, ranged.Min, ranged.Max))
                {
                    return EStringConversionResult.OutOfRange;
                }

                result = value;
                return EStringConversionResult.Success;
            }

            if (type == Int)
            {
                if (!value.TryConvert(out int r))
                {
                    return EStringConversionResult.Invalid;
                }

                if (!Numeric.IsWithinRange(r, ranged.Min, ranged.Max))
                {
                    return EStringConversionResult.OutOfRange;
                }

                result = r;
                return EStringConversionResult.Success;
            }

            if (type == Float)
            {
                if (!value.TryConvert(out float r))
                {
                    return EStringConversionResult.Invalid;
                }

                if (!r.IsWithinRange<float>(ranged.Min, ranged.Max))
                {
                    return EStringConversionResult.OutOfRange;
                }

                result = r;
                return EStringConversionResult.Success;
            }
        }

        if (type == Bool)
        {
            if (!bool.TryParse(value, out bool r))
            {
                return EStringConversionResult.Invalid;
            }

            result = r;
            return EStringConversionResult.Success;
        }

        if (type == Constant && value == commandType.Name)
        {
            result = value;
            return EStringConversionResult.Success;
        }

        if (type == ECommandInputType.Enum && commandType is CommandTypeEnum ce)
        {
            if (!ce.Values.TryGetValue(value, out int v))
            {
                return EStringConversionResult.Invalid;
            }

            result = v;
            return EStringConversionResult.Success;
        }

        return EStringConversionResult.Invalid;
    }
}
