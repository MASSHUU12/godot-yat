using System;
using System.Collections.Generic;
using System.Linq;
using Confirma.Extensions;
using Godot;

using static System.StringComparison;
using static Confirma.Terminal.EArgumentParseResult;

namespace Confirma.Terminal;

public class Cli(string prefix = "")
{
    private readonly string _prefix = prefix;
    private readonly Dictionary<string, Argument> _arguments = new(
        StringComparer.OrdinalIgnoreCase
    );
    private readonly Dictionary<string, object?> _argumentValues = [];

    public Argument? GetArgument(string name)
    {
        _ = _arguments.TryGetValue(name, out Argument? argument);
        return argument;
    }

    public T? GetArgumentValue<T>(string name)
    {
        return _argumentValues.TryGetValue(
                name,
                out object? value
            )
            && value is T typedValue
                ? typedValue
                : default;
    }

    public bool IsFlagSet(string name)
    {
        return GetArgumentValue<bool>(name) is bool isSet && isSet;
    }

    public int GetValuesCount()
    {
        return _argumentValues.Count;
    }

    public bool RegisterArgument(params Argument[] arguments)
    {
        foreach (Argument argument in arguments)
        {
            if (!_arguments.TryAdd(argument.Name, argument))
            {
                return false;
            }
        }

        return true;
    }

    public bool InvokeArgumentAction(string name)
    {
        Argument? argument = GetArgument(name);

        if (argument is null)
        {
            return false;
        }

        argument.Invoke(GetArgumentValue<object>(name)!);

        return true;
    }

    public List<string> Parse(string[] args, bool invokeActions = false)
    {
        List<string> errors = [];

        for (int i = 0; i < args.Length; i++)
        {
            (string argName, string? argValue) = ParseArgumentString(args[i]);

            bool hasPrefix = ArgumentHasPrefix(argName);
            string normalizedName = NormalizeArgumentName(argName);

            if (!_arguments.TryGetValue(normalizedName, out Argument? argument))
            {
                errors.Add(GenerateErrorForInvalidArgument(argName));
                continue;
            }

            if (argument.UsePrefix && !hasPrefix)
            {
                errors.Add(
                    $"Argument '{argument.Name}' requires the prefix '{_prefix}'."
                );
                continue;
            }

            if (!argument.UsePrefix && hasPrefix)
            {
                errors.Add(
                    $"Argument '{argument.Name}' should not have the prefix '{_prefix}'."
                );
                continue;
            }

            EArgumentParseResult argResult = argument.Parse(argValue, out object? parsed);

            if (argResult is not Success)
            {
                errors.Add(GenerateErrorForArgumentParsingFailure(argument, argResult));
                continue;
            }

            _argumentValues[argument.Name] = parsed;

            if (invokeActions)
            {
                argument.Invoke(parsed!);
            }
        }

        return errors;
    }

    private string NormalizeArgumentName(string argName)
    {
        return ArgumentHasPrefix(argName)
            ? argName[_prefix.Length..]
            : argName;
    }

    private bool ArgumentHasPrefix(string argName)
    {
        return argName.StartsWith(_prefix, OrdinalIgnoreCase);
    }

    private string? FindSimilarArgument(string name)
    {
        const double minSimilarity = 0.88;
        double maxSimilarity = double.MinValue;
        string? similarArgument = null;

        IEnumerable<string>? candidates = _arguments.Keys.Where(key =>
            key.StartsWith(name[0])
            || Math.Abs(key.Length - name.Length) <= maxSimilarity
        );

        foreach (string key in candidates)
        {
            double currentSimilarity = name.JaroWinklerSimilarity(key);

            if (currentSimilarity <= maxSimilarity)
            {
                continue;
            }

            maxSimilarity = currentSimilarity;
            similarArgument = key;
        }

        return maxSimilarity >= minSimilarity ? similarArgument : null;
    }

    private string GenerateErrorForArgumentParsingFailure(
        Argument arg,
        EArgumentParseResult result
    )
    {
        string fullName = arg.UsePrefix ? _prefix + arg.Name : arg.Name;

        return result switch
        {
            Success => string.Empty,
            ValueRequired => $"Value for '{fullName}' cannot be empty.",
            UnexpectedValue =>
                $"'{fullName}' is a flag and doesn't accept any value.",
            _ => $"An error occurred while parsing '{fullName}'."
        };
    }

    private string GenerateErrorForInvalidArgument(string name)
    {
        string? similarArgument = FindSimilarArgument(
            NormalizeArgumentName(name)
        );

        return $"Unknown argument: {name}."
            + (
                string.IsNullOrEmpty(similarArgument)
                    ? string.Empty
                    : $" Did you mean '{similarArgument}'?"
            );
    }

    private static (string, string?) ParseArgumentString(string argument)
    {
        if (argument.Find('=') == -1)
        {
            return (argument, null);
        }

        string[] split = argument.Split('=', 2);

        return (split[0], split[1]);
    }
}
